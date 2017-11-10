using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Download.CustomEventArgs;
using HqDownloadManager.Download.Databases;
using HqDownloadManager.Download.Models;
using HqDownloadManager.Utils;

namespace HqDownloadManager.Download {
    public class DownloadManager {
        private readonly Downloader _downloader;
        private readonly TaskTimer _timerHelper;

        private readonly object _lockThis = new object();
        private readonly object _lockThis2 = new object();

        public event DownloadEventHandler DownloadStart;
        public event DownloadEventHandler DownloadEnd;
        public event ProgressEventHandler DownloadProgress;
        public event ProgressEventHandler DownloadPause;
        public event ProgressEventHandler DownloadResume;
        public event DownloadErrorEventHandler DownloadError;

        public DownloadManager(DependencyInjection dependency) : this(AppDomain.CurrentDomain.BaseDirectory, dependency) {

        }

        public DownloadManager(string cacheDirectory, DependencyInjection dependency) {
            var dependency1 = dependency;
            this._timerHelper = dependency1.Resolve<TaskTimer>();
            _downloader = dependency1
                .DefineDependency<DownloadContext>(0, cacheDirectory)
                .Resolve<Downloader>();
            _downloader.DownloadStart += Downloader_DownloadStart;
            _downloader.DownloadProgress += Downloader_DownloadProgress;
            _downloader.DownloadPause += Downloader_DownloadPause;
            _downloader.DownloadResume += Downloader_DownloadResume;
            _downloader.DownloadError += Downloader_DownloadError;
            _downloader.DownloadEnd += Downloader_DownloadEnd;
        }

        public void Download(ModelBase item, string directory) {
            lock (_lockThis) {
                switch (item) {
                    case Hq hq:
                        _downloader.SaveHq(hq, directory);
                        break;
                    case Chapter chapter:
                        _downloader.SaveChapter(chapter, directory);
                        break;
                }
            }
        }

        public void PauseResumeDownload(bool state) => _downloader.PauseRemumeDownload(state);

        public List<HqDownloadInfo> GetDownloadedHqsInfo() => _downloader.GetDownloadedHqsInfo();


        private void Downloader_DownloadStart(object sender, DownloadEventArgs ev) => DownloadStart?.Invoke(this, ev);

        private void Downloader_DownloadProgress(object sender, ProgressEventArgs ev) => DownloadProgress?.Invoke(this, ev);

        private void Downloader_DownloadPause(object sender, ProgressEventArgs ev) => DownloadPause?.Invoke(this, ev);

        private void Downloader_DownloadResume(object sender, ProgressEventArgs ev) => DownloadResume?.Invoke(this, ev);

        private void Downloader_DownloadError(object sender, DownloadErrorEventArgs ev) => DownloadError?.Invoke(this, ev);

        private void Downloader_DownloadEnd(object sender, DownloadEventArgs ev) => DownloadEnd?.Invoke(this, ev);
    }
}
