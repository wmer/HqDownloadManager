using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HqDownloadManager.Core;
using HqDownloadManager.Download.CustomEventArgs;
using HqDownloadManager.Download.Databases;
using HqDownloadManager.Download.Helpers;
using HqDownloadManager.Download.Models;
using HqDownloadManager.Utils;
using Newtonsoft.Json;

namespace HqDownloadManager.Download {
    internal class Downloader {
        private readonly DirectoryHelper _directoryHelper;
        private readonly TaskTimer _timerHelper;
        private readonly SourceManager _sourceManager;
        private readonly DownloadInfoHelper _downloadInfoHelper;
        private bool _processing;
        private bool _paused;

        private readonly object _lock1 = new object();
        private readonly object _lock2 = new object();
        private readonly object _lock5 = new object();
        private readonly object _lock6 = new object();
        private readonly object _lock9 = new object();

        public event DownloadEventHandler DownloadStart;
        public event DownloadEventHandler DownloadEnd;
        public event ProgressEventHandler DownloadProgress;
        public event ProgressEventHandler DownloadPause;
        public event ProgressEventHandler DownloadResume;
        public event DownloadErrorEventHandler DownloadError;

        public Downloader(DirectoryHelper directoryHelper, TaskTimer timerHelper, SourceManager sourceManager, DownloadInfoHelper downloadInfoHelper) {
            this._directoryHelper = directoryHelper;
            this._timerHelper = timerHelper;
            this._sourceManager = sourceManager;
            this._downloadInfoHelper = downloadInfoHelper;
        }

        public void SaveHq(Hq hqInfo, string directory) {
            lock (_lock1) {
                var startTime = DateTime.Now;
                var hqDirectory = _directoryHelper.CreateHqDirectory(directory, hqInfo.Title);
                DownloadStart(this, new DownloadEventArgs(hqInfo, new DirectoryInfo(hqDirectory), startTime));
                var tempHq = new Hq {
                    Link = hqInfo.Link, Title = hqInfo.Title, CoverSource = hqInfo.CoverSource,
                    Synopsis = hqInfo.Synopsis, Chapters = new List<Chapter>()
                };
                var numChapters = hqInfo.Chapters.Count();
                var chapAtual = 1;
                var failedToDownload = new List<String>();
                var totalTime = _timerHelper.RuntimeOf(() => {
                    DownloadProgress(this, new ProgressEventArgs(hqInfo, 0, numChapters));
                    foreach (var chapter in hqInfo.Chapters) {
                        try {
                            if (chapter.Pages == null || chapter.Pages.Count == 0) {
                                if (!_processing) {
                                    Task.Run(() => {
                                        AddPagesInChapters(ref hqInfo);
                                    });
                                }

                                var chapterInfo = (Chapter)_sourceManager.GetInfo(chapter.Link);
                                chapter.Pages = chapterInfo?.Pages;
                            }
                            SaveChapter(chapter, hqDirectory);
                            tempHq.Chapters.Add(chapter);
                            _downloadInfoHelper.SaveHqDownloadInfo(tempHq, hqDirectory, startTime);
                        } catch (Exception e) {
                            DownloadError(this, new DownloadErrorEventArgs(chapter, e, DateTime.Now));
                            failedToDownload.Add(chapter.Link);
                        }
                        DownloadProgress(this, new ProgressEventArgs(hqInfo, chapAtual, numChapters));
                        chapAtual++;
                    }
                });

                _downloadInfoHelper.SaveHqDownloadInfo(tempHq, hqDirectory, startTime);
                DownloadEnd(this, new DownloadEventArgs(hqInfo, new DirectoryInfo(hqDirectory), startTime, DateTime.Now, totalTime, failedToDownload));
            }
        }

        public void SaveChapter(Chapter chapter, string directory) {
            lock (_lock2) {
                var startChapterDownload = DateTime.Now;
                var chapterDirectory = _directoryHelper.CreateHqDirectory(directory, chapter.Title);
                DownloadStart(this, new DownloadEventArgs(chapter, new DirectoryInfo(chapterDirectory), startChapterDownload));
                var downloadChapterTime = _timerHelper.RuntimeOf(() => {
                    var pageAtual = 1;
                    var totalPages = chapter.Pages.Count();
                    DownloadProgress(this, new ProgressEventArgs(chapter, 0, totalPages));
                    foreach (var page in chapter.Pages) {
                        if (_paused) {
                            DownloadPause(this, new ProgressEventArgs(DateTime.Now, chapter, pageAtual, totalPages));
                            while (_paused) ;
                            DownloadResume(this, new ProgressEventArgs(DateTime.Now, chapter, pageAtual, totalPages));
                        }
                        try {
                            var pageSource = $"{chapterDirectory}\\{page.Number.ToString().PadLeft(3, '0')}{FormatPage(page.Source)}";
                            if (!File.Exists(pageSource)) {
                                using (var webClient = new WebClient()) {
                                   webClient.DownloadFile(new Uri(page.Source), pageSource);
                                }
                            }
                            page.Source = pageSource;
                        } catch (Exception e) {
                            DownloadError(this, new DownloadErrorEventArgs(null, e, DateTime.Now));
                        }

                        DownloadProgress(this, new ProgressEventArgs(chapter, pageAtual, totalPages));
                        pageAtual++;
                    }
                });

                DownloadEnd(this, new DownloadEventArgs(chapter, new DirectoryInfo(chapterDirectory),
                   startChapterDownload, DateTime.Now, downloadChapterTime));
            }
        }

        private string FormatPage(string source) {
            lock (_lock9) {
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

        public void PauseRemumeDownload(bool state) {
            _paused = state;
        }

        public List<HqDownloadInfo> GetDownloadedHqsInfo() {
            lock (_lock6) {
                return _downloadInfoHelper.GetHqsDownloadInfo();
            }
        }

        private void AddPagesInChapters(ref Hq hq) {
            lock (_lock5) {
                _processing = true;
                foreach (var chapter in hq.Chapters) {
                    var chapterInfo = (Chapter)_sourceManager.GetInfo(chapter.Link);
                    chapter.Pages = chapterInfo?.Pages;
                }
                _processing = false;
            }
        }
    }
}
