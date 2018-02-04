using DependencyInjectionResolver;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download.CustomEventArgs;
using HqDownloadManager.Download.Helpers;
using HqDownloadManager.Download.Models;
using Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace HqDownloadManager.Download {
    public class DownloadManager {
        private readonly Downloader _downloader;
        private readonly DownloadInfoHelper _downloadInfoHelper;

        public DownloadManager(DependencyInjection dependency) {
            var dependency1 = dependency;
            _downloader = dependency1.Resolve<Downloader>();
            _downloadInfoHelper = dependency1.Resolve<DownloadInfoHelper>();
        }

        public void AddToDownloadList(Hq hq, string directory) =>
                        _downloader.AddToDownloadList(hq, directory);

        public void ExcludeFromDownloadList(DownloadItem item) =>
                        _downloader.ExcludeFromDownloadList(item);

        public ObservableCollection<DownloadItem> GetDownloadList() =>
                        _downloader.GetDownloadList();

        public void Download() =>
                        _downloader.Download();

        public void Download(DownloadItem downloadItem) =>
                        _downloader.Download(downloadItem);

        public void PauseResumeDownload() =>
                        _downloader.PauseRemumeDownload();

        public void StopDownload() =>
                        _downloader.StopDownload();

        public List<HqDownloadInfo> GetDownloadedHqsInfo() =>
                        _downloadInfoHelper.GetHqsDownloadInfo();

        public HqDownloadInfo GetChaptersFromDownloadInfo(HqDownloadInfo downloadInfo) =>
                        _downloadInfoHelper.GetChaptersFromDownloadInfo(downloadInfo);

        public void SaveDownloadInfo(HqDownloadInfo downloadInfo) =>
                        _downloadInfoHelper.SaveDownloadInfo(downloadInfo);

        public void DeleteDownloadInfo(HqDownloadInfo downloadInfo, bool deleteFiles = false) =>
              _downloadInfoHelper.DeleteDownloadInfo(downloadInfo, deleteFiles);

    }
}
