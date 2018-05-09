using DependencyInjectionResolver;
using HqDownloadManager.Database;
using HqDownloadManager.Models;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HqDownloadManager.Download {
    public class DownloadManager {
        private readonly Downloader _downloader;

        public DownloadManager(DependencyInjection dependency) {
            var dependency1 = dependency;
            _downloader = dependency1.Resolve<Downloader>();
        }

        public void AddToDownloadList(Hq hq, string directory) =>
                        _downloader.AddToDownloadList(hq, directory);

        public void ExcludeFromDownloadList(Hq item) =>
                        _downloader.ExcludeFromDownloadList(item);

        public ObservableCollection<Hq> GetDownloadList() =>
                        _downloader.GetDownloadList();

        public void Download() =>
                        _downloader.Download();

        public void Download(DownloadItem downloadItem) =>
                        _downloader.Download(downloadItem);

        public void PauseResumeDownload() =>
                        _downloader.PauseRemumeDownload();

        public void StopDownload() =>
                        _downloader.StopDownload();
    }
}
