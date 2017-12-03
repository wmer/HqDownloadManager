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

namespace HqDownloadManager.Download
{
    public class DownloadManager {
        private readonly Downloader _downloader;
        private readonly TaskTimer _timerHelper;
        private readonly DownloadInfoHelper _downloadInfoHelper;

        public event DownloadEventHandler DownloadStart;
        public event DownloadEventHandler DownloadEnd;
        public event ProgressEventHandler DownloadProgress;
        public event ProgressEventHandler DownloadPause;
        public event ProgressEventHandler DownloadResume;
        public event DownloadErrorEventHandler DownloadError;

        public DownloadManager(DependencyInjection dependency) {
            var dependency1 = dependency;
            this._timerHelper = dependency1.Resolve<TaskTimer>();
            _downloader = dependency1.Resolve<Downloader>();
            _downloadInfoHelper = dependency1.Resolve<DownloadInfoHelper>();
            _downloader.DownloadStart += Downloader_DownloadStart;
            _downloader.DownloadProgress += Downloader_DownloadProgress;
            _downloader.DownloadPause += Downloader_DownloadPause;
            _downloader.DownloadResume += Downloader_DownloadResume;
            _downloader.DownloadError += Downloader_DownloadError;
            _downloader.DownloadEnd += Downloader_DownloadEnd;
        }

        public void Download(Hq item, string directory) =>
                        _downloader.SaveHq(item, directory);

        public void PauseResumeDownload(bool state) => 
                        _downloader.PauseRemumeDownload(state);

        public async Task<List<HqDownloadInfo>> GetDownloadedHqsInfo() =>
                        await _downloadInfoHelper.GetHqsDownloadInfo();
        

        public async Task DeleteDownloadInfo(string link, bool deleteFiles = false) =>
              await _downloadInfoHelper.DeleteDownloadInfo(link, deleteFiles);

        private void Downloader_DownloadStart(object sender, DownloadEventArgs ev) => DownloadStart?.Invoke(this, ev);

        private void Downloader_DownloadProgress(object sender, ProgressEventArgs ev) => DownloadProgress?.Invoke(this, ev);

        private void Downloader_DownloadPause(object sender, ProgressEventArgs ev) => DownloadPause?.Invoke(this, ev);

        private void Downloader_DownloadResume(object sender, ProgressEventArgs ev) => DownloadResume?.Invoke(this, ev);

        private void Downloader_DownloadError(object sender, DownloadErrorEventArgs ev) => DownloadError?.Invoke(this, ev);

        private void Downloader_DownloadEnd(object sender, DownloadEventArgs ev) => DownloadEnd?.Invoke(this, ev);
    }
}
