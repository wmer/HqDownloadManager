using HqDownloadManager.Database;
using HqDownloadManager.Events;
using HqDownloadManager.Helpers;
using HqDownloadManager.Models;
using MangaScraping;
using MangaScraping.Managers;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Download {
    internal class Downloader {
        private static ObservableCollection<DownloadItem> _downloadList = new ObservableCollection<DownloadItem>();

        private readonly DirectoryHelper _directoryHelper;
        private readonly TaskTimer _timerHelper;
        private readonly SourceManager _sourceManager;
        private readonly DownloadManagerContext _downloadContext;
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

        public Downloader(DirectoryHelper directoryHelper, TaskTimer timerHelper, SourceManager sourceManager, DownloadManagerContext downloadContext) {
            this._directoryHelper = directoryHelper;
            this._timerHelper = timerHelper;
            this._sourceManager = sourceManager;
            this._downloadContext = downloadContext;

            var list = _downloadContext.DownloadList.FindAll();
            if (list != null || list.Count > 0) {
                foreach (var item in list) {
                    _downloadList.Add(item);
                }
            }
        }

        public void AddToDownloadList(Hq hq, string directory) {
            lock (_lock1) {
                if (hq != null && !string.IsNullOrEmpty(hq.Link)) {
                    var downloaditem = new DownloadItem { Hq = hq.ToBytes(), DownloadLocation = directory, IsDownloaded = false };
                    if (!_downloadList.Contains(downloaditem)) {
                        _downloadList.Add(downloaditem);
                        _downloadContext.DownloadList.Save(downloaditem);
                        downloaditem.Id = _downloadContext.DownloadList.Find()
                                            .Where(x => x.DownloadLocation == directory)
                                            .Execute().FirstOrDefault().Id;
                    }
                }
            }
        }

        public void ExcludeFromDownloadList(Hq item) {
            lock (_lock2) {
                foreach (var itemToDownload in _downloadList) {
                    if (itemToDownload.Hq.ToObject<Hq>() == item) {
                        _downloadContext.DownloadList.Delete(itemToDownload);
                    }
                }
            }
        }

        public ObservableCollection<Hq> GetDownloadList() {
            var listHq = new ObservableCollection<Hq>();
            foreach (var item in _downloadList) {
                listHq.Add(item.Hq.ToObject<Hq>());
            }
            return listHq;
        }

        public void Download() {
            lock (_lock3) {
                Task.Run(() => {
                    _stop = false;
                    _paused = false;
                    for (var i = 0; i < _downloadList.Count; i++) {
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
                var source = _sourceManager.GetSourceFromLink(downloadItem.Hq.ToObject<Hq>().Link);
                if (downloadItem.Hq.ToObject<Hq>().Chapters == null || downloadItem.Hq.ToObject<Hq>().Chapters.Count() == 0) {
                    var hq = new Hq();
                    source.GetInfo(downloadItem.Hq.ToObject<Hq>().Link, out hq);
                    if (hq != null) {
                        downloadItem.Hq = hq.ToBytes();
                    }
                }

                downloadItem.DownloadStarted = DateTime.Now;
                DownloadEventHub.OnDownloadStart(this, new DownloadEventArgs(downloadItem));

                var hqDirectory = _directoryHelper.CreateHqDirectory(downloadItem.DownloadLocation, downloadItem.Hq.ToObject<Hq>().Title);
                var numChapters = downloadItem.Hq.ToObject<Hq>().Chapters.Count();
                var chapAtual = 1;
                var failedToDownload = new List<String>();
                var downloadedChapters = new List<DownloadedChapter>();

                foreach (var chapter in downloadItem.Hq.ToObject<Hq>().Chapters) {
                    if (_stop) {
                        DownloadEventHub.OnDownloadStop(this, new DownloadEventArgs(downloadItem, (downloadItem.DownloadFinished - downloadItem.DownloadStarted), failedToDownload));
                        break;
                    }
                    try {
                        SaveChapter(source, downloadItem, chapter, chapAtual, numChapters, hqDirectory);
                    } catch (Exception e) {
                        DownloadEventHub.OnDownloadError(this, new DownloadErrorEventArgs(downloadItem, e, DateTime.Now));
                        failedToDownload.Add(chapter.Link);
                    }

                    var chapterDirectory = _directoryHelper.CreateHqDirectory(hqDirectory, chapter.Title);
                    downloadedChapters.Add(new DownloadedChapter {
                        Location = chapterDirectory, Date = DateTime.Now
                    });
                    chapAtual++;
                }
                downloadItem.DownloadFinished = DateTime.Now;
                var hqDownloaded = new DownloadedHq() {
                    Location = downloadItem.DownloadLocation,
                    Date = DateTime.Now,
                    Chapters = downloadedChapters
                };
                downloadItem.IsDownloaded = true;
                _downloadContext.DownloadList.Update(downloadItem);
                SaveDownloadedHq(hqDownloaded);
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
                DownloadEventHub.OnDownloadChapterStart(this, new ProgressEventArgs(DateTime.Now, downloadItem, chpaterIndex, totalChapters));

                var pageAtual = 1;
                var totalPages = chapter.Pages.Count();
                foreach (var page in chapter.Pages) {
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

        private void SaveDownloadedHq(DownloadedHq downloadedHq) {
            lock (_lock7) {
                _downloadContext.DownloadedHQ.Save(downloadedHq);
                downloadedHq.Id = _downloadContext.DownloadedHQ.Find()
                                            .Where(x => x.Location == downloadedHq.Location)
                                            .Execute().FirstOrDefault().Id;
                foreach (var chap in downloadedHq.Chapters) {
                    chap.DownloadedHq = downloadedHq;
                }
                _downloadContext.DownloadedChapter.Save(downloadedHq.Chapters);
            }
        }
    }
}
