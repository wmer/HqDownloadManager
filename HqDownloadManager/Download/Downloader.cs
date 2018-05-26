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
        private readonly DownloadHelper _downloadHelper;
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
        private readonly object _lock9 = new object();

        public Downloader(DirectoryHelper directoryHelper, TaskTimer timerHelper, SourceManager sourceManager, DownloadManagerContext downloadContext, DownloadHelper downloadHelper) {
            this._directoryHelper = directoryHelper;
            this._timerHelper = timerHelper;
            this._sourceManager = sourceManager;
            this._downloadContext = downloadContext;
            this._downloadHelper = downloadHelper;

            var list = _downloadContext.DownloadList.FindAll();
            if (list != null || list.Count > 0) {
                foreach (var item in list) {
                    _downloadList.Add(item);
                }
            }
        }

        public void AddToDownloadList(Hq hq, string directory) {
            lock (_lock1) {
                if (!string.IsNullOrWhiteSpace(directory) && hq != null && !string.IsNullOrEmpty(hq.Link)) {
                    hq.Chapters = null;
                    var downloaditem = new DownloadItem { Hq = hq.ToBytes(), DownloadLocation = directory, IsDownloaded = false };
                    if (!_downloadList.Contains(downloaditem)) {
                        _downloadList.Add(downloaditem);
                        _downloadContext.DownloadList.Save(downloaditem);
                        downloaditem.Id = _downloadContext.DownloadList.Find()
                                            .Where(x => x.DownloadLocation == directory)
                                            .Execute().FirstOrDefault().Id;
                        SaveLocation(directory);
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

        public ObservableCollection<DownloadItem> GetDownloadList() => _downloadList;

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
                var hq = downloadItem.Hq.ToObject<Hq>();
                var source = _sourceManager.GetSourceFromLink(hq.Link);
                if (hq.Chapters == null || hq.Chapters.Count() == 0) {
                    source.GetInfo(hq.Link, out hq);
                }

                downloadItem.DownloadStarted = DateTime.Now;
                DownloadEventHub.OnDownloadStart(this, new DownloadEventArgs(downloadItem));

                var hqDirectory = _directoryHelper.CreateHqDirectory(downloadItem.DownloadLocation, hq.Title);
                var numChapters = hq.Chapters.Count();
                downloadItem.NumChapters = numChapters;
                var chapAtual = 1;
                var failedToDownload = new List<String>();
                var coverSource = hq.CoverSource;
                var hqTitle = StringHelper.RemoveSpecialCharacters(hq.Title);
                hq.Title = hqTitle;

                coverSource = $"{hqDirectory}\\{hqTitle}{FormatPage(coverSource)}";
                if (!File.Exists(coverSource)) {
                    using (var webClient = new HttpClient()) {
                        using (var response = webClient.GetAsync(hq.CoverSource).Result) {
                            var imageByte = response.Content.ReadAsByteArrayAsync().Result;
                            using (var binaryWriter = new BinaryWriter(new FileStream(coverSource,
                                                                          FileMode.Append, FileAccess.Write))) {
                                binaryWriter.Write(imageByte);
                            }
                        }
                    }
                }
                hq.CoverSource = coverSource;

                foreach (var chapter in hq.Chapters) {
                    downloadItem.ActualChapterIndex = chapAtual - 1;
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
                    chapAtual++;
                }
                downloadItem.DownloadFinished = DateTime.Now;
                var hqDownloaded = new DownloadedHq() {
                    Hq = hq,
                    Location = hqDirectory,
                    Date = DateTime.Now
                };
                downloadItem.IsDownloaded = true;
                //downloadItem.Hq = hq.ToBytes();
                _downloadHelper.SaveDownloadedHq(hqDownloaded);
                _downloadContext.DownloadList.Update(downloadItem);
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
                downloadItem.NumPages = totalPages;
                foreach (var page in chapter.Pages) {
                    downloadItem.ActualPageIndex = pageAtual - 1;
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

        public void SaveLocation(string directory) {
            lock (_lock9) {
                var locations = _downloadContext.DownloadLocation.FindAll();
                var contains = false;
                foreach (var loc in locations) {
                    if (loc.Location == directory) {
                        contains = true;
                    }
                }
                if (!contains) {
                    _downloadContext.DownloadLocation.Save(new DownloadLocation { Location = directory });
                }
            }
        }

        public void PauseRemumeDownload() => _paused = !_paused;
        public void StopDownload() => _stop = true;
    }
}
