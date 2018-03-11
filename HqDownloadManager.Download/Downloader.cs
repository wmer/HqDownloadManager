using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download.CustomEventArgs;
using HqDownloadManager.Download.Helpers;
using HqDownloadManager.Download.Models;
using Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Download.Databases;
using System.Collections.ObjectModel;
using HqDownloadManager.Core.Managers;
using System.Net.Http;

namespace HqDownloadManager.Download {
    internal class Downloader {
        private static ObservableCollection<DownloadItem> _downloadList = new ObservableCollection<DownloadItem>();

        private readonly DirectoryHelper _directoryHelper;
        private readonly TaskTimer _timerHelper;
        private readonly SourceManager _sourceManager;
        private readonly DownloadContext _downloadContext;
        private readonly DownloadInfoHelper _downloadInfoHelper;
        private bool _paused;
        private bool _stop;

        private readonly object _lock1 = new object();
        private readonly object _lock2 = new object();
        private readonly object _lock3 = new object();
        private readonly object _lock4 = new object();
        private readonly object _lock5 = new object();
        private readonly object _lock6 = new object();
        private readonly object _lock7 = new object();
        private readonly object _lock8 = new object();

        public Downloader(DirectoryHelper directoryHelper, TaskTimer timerHelper, SourceManager sourceManager, DownloadInfoHelper downloadInfoHelper, DownloadContext downloadContext) {
            this._directoryHelper = directoryHelper;
            this._timerHelper = timerHelper;
            this._sourceManager = sourceManager;
            this._downloadInfoHelper = downloadInfoHelper;
            this._downloadContext = downloadContext;

            var list = _downloadContext.DownloadList.FindAll();
            if (list != null || list.Count > 0) {
                foreach (var item in list) {
                    if (item.Hq != null && !string.IsNullOrEmpty(item.Hq.Link)) {
                        _downloadList.Add(item);
                    }
                }
            }
        }

        public void AddToDownloadList(Hq hq, string directory) {
            lock (_lock1) {
                foreach (var chap in hq.Chapters) {
                    chap.ToDownload = true;
                }
                _downloadContext.Chapter.Update(hq.Chapters);
                if (hq != null && !string.IsNullOrEmpty(hq.Link)) {
                    var downloaditem = new DownloadItem { Hq = hq, DownloadLocation = directory, IsDownloaded = false };
                    if (!_downloadList.Contains(downloaditem)) {
                        _downloadList.Add(downloaditem);
                        _downloadContext.DownloadList.Save(downloaditem);
                    }
                }
            }
        }

        public void ExcludeFromDownloadList(DownloadItem item) {
            lock (_lock2) {
                var hq = item.Hq;
                _downloadList.Remove(item);
                _downloadContext.DownloadList.Delete(item);
                if ((_downloadContext.Chapter.Find().Where(x => x.Hq == hq && x.ToDownload == true).Execute() is List<Chapter> chaps) && chaps.Count > 0) {
                    foreach (var chap in chaps) {
                        chap.ToDownload = false;
                    }
                    _downloadContext.Chapter.Update(chaps);
                }
            }
        }

        public ObservableCollection<DownloadItem> GetDownloadList() => _downloadList;

        public void Download() {
            lock (_lock3) {
                Task.Run(() => {
                    _stop = false;
                    _paused = false;
                    for(var i = 0; i < _downloadList.Count; i++) {
                        var item = _downloadList[i];
                        if (_stop) {
                            break;
                        }
                        if (!item.IsDownloaded) {
                            Download(item);
                        }
                    }
                });
            }
        }

        public void Download(DownloadItem downloadItem) {
            lock (_lock4) {
                var source = _sourceManager.GetSourceFromLink(downloadItem.Hq.Link);
                if (downloadItem.Hq.Chapters == null || downloadItem.Hq.Chapters.Count() == 0) {
                    var hq = downloadItem.Hq;
                    if ((_downloadContext.Chapter.Find().Where(x => x.Hq == hq && x.ToDownload == true).Execute() is List<Chapter> chaps) && chaps.Count > 0) {
                        downloadItem.Hq.Chapters = chaps;
                    }else {
                        hq = new Hq();
                        source.GetInfo(downloadItem.Hq.Link, out hq);
                        if (hq != null) {
                            downloadItem.Hq = hq;
                        }
                    }
                }

                downloadItem.DownloadStarted = DateTime.Now;
                DownloadEventHub.OnDownloadStart(this, new DownloadEventArgs(downloadItem));

                var hqDirectory = _directoryHelper.CreateHqDirectory(downloadItem.DownloadLocation, downloadItem.Hq.Title);
                var numChapters = downloadItem.Hq.Chapters.Count();
                var chapAtual = 1;
                var failedToDownload = new List<String>();
                foreach (var chapter in downloadItem.Hq.Chapters) {
                    if (_stop) {
                        DownloadEventHub.OnDownloadStop(this, new DownloadEventArgs(downloadItem, (downloadItem.DownloadFinished - downloadItem.DownloadStarted), failedToDownload));
                        break;
                    }
                    try {
                        downloadItem.ActualPage = null;
                        SaveChapter(source, downloadItem, chapter, chapAtual, numChapters, hqDirectory);
                    } catch (Exception e) {
                        DownloadEventHub.OnDownloadError(this, new DownloadErrorEventArgs(downloadItem, e, DateTime.Now));
                        failedToDownload.Add(chapter.Link);
                    }
                    chapAtual++;
                }
                downloadItem.DownloadFinished = DateTime.Now;
                var downloadInfo = new HqDownloadInfo(downloadItem) {
                    Path = hqDirectory
                };
                downloadItem.IsDownloaded = true;
                _downloadContext.DownloadList.Save(downloadItem);
                _downloadInfoHelper.SaveDownloadInfo(downloadInfo);
                DownloadEventHub.OnDownloadEnd(this, new DownloadEventArgs(downloadItem, (downloadItem.DownloadFinished - downloadItem.DownloadStarted), failedToDownload));
            }
        }

        private void SaveChapter(IHqSourceManager source, DownloadItem downloadItem, Chapter chapter, int chpaterIndex, int totalChapters, string directory) {
            lock (_lock5) {
                var startChapterDownload = DateTime.Now;
                var chapterDirectory = _directoryHelper.CreateHqDirectory(directory, chapter.Title);
                if (chapter.Pages == null || chapter.Pages.Count == 0) {
                    var chapterInfo = new Chapter();
                    source.GetInfo(chapter.Link, out chapterInfo);
                    chapter.Pages = chapterInfo.Pages;
                }
                downloadItem.LastDownloadedChapter = chapter;
                DownloadEventHub.OnDownloadChapterStart(this, new ProgressEventArgs(DateTime.Now, downloadItem, chpaterIndex, totalChapters));

                var pageAtual = 1;
                var totalPages = chapter.Pages.Count();
                foreach (var page in chapter.Pages) {
                    downloadItem.ActualPage = page;
                    if (_stop) {
                        break;
                    }
                    if (_paused) {
                        DownloadEventHub.OnDownloadPause(this, new ProgressEventArgs(DateTime.Now, downloadItem, pageAtual, totalPages));
                        while (_paused) ;
                        DownloadEventHub.OnDownloadResume(this, new ProgressEventArgs(DateTime.Now, downloadItem, pageAtual, totalPages));
                    }

                    DownloadEventHub.OnDownloadProgress(this, new ProgressEventArgs(DateTime.Now, downloadItem, pageAtual, totalPages));

                    try {
                        var pageSource = $"{chapterDirectory}\\{page.Number.ToString().PadLeft(3, '0')}{FormatPage(page.Source)}";
                        if (!File.Exists(pageSource)) {
                            ServicePointManager.DefaultConnectionLimit = 1000;
                            using (var webClient = new HttpClient()) {
                                //webClient.Proxy = null;
                                //webClient.DownloadFile(page.Source, pageSource);
                                using (var response = webClient.GetAsync(page.Source).Result) {
                                    var imageByte = response.Content.ReadAsByteArrayAsync().Result;
                                    using (var binaryWriter = new BinaryWriter(new FileStream(pageSource,
                                                                                  FileMode.Append, FileAccess.Write))) {
                                        binaryWriter.Write(imageByte);
                                    }
                                }
                            }
                        }

                        page.Source = pageSource;
                        page.Chapter = chapter;
                    } catch (Exception e) {
                        DownloadEventHub.OnDownloadError(this, new DownloadErrorEventArgs(null, e, DateTime.Now));
                    }

                    pageAtual++;
                }

                DownloadEventHub.OnDownloadChapterEnd(this, new DownloadEventArgs(downloadItem));
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private string FormatPage(string source) {
            lock (_lock6) {
                var formatPosition = source.LastIndexOf(".");
                var format = source.Substring(formatPosition);
                if (format.Contains("?")) {
                    var posi = format.IndexOf("?");
                    var after = format.Substring(posi);
                    format = format.Replace(after, "");
                }

                if (format.Contains("&")) {
                    var posi = format.IndexOf("&");
                    var after = format.Substring(posi);
                    format = format.Replace(after, "");
                }

                return format;
            }
        }

        public void PauseRemumeDownload() => _paused = !_paused;
        public void StopDownload() => _stop = true;
    }
}
