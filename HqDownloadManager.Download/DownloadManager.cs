using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Download.CustomEventArgs;
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
        public event FollowEventHandler FollowingHq;
        public event UpdateEventHandler UpdateHq;
        public event UpdateEventHandler UpdateStart;
        public event UpdateEventHandler UpdateEnd;

        public DownloadManager(DependencyInjection dependency) {
            var dependency1 = dependency;
            this._timerHelper = dependency1.Resolve<TaskTimer>();
            _downloader = dependency1.Resolve<Downloader>();
            _downloader.DownloadStart += Downloader_DownloadStart;
            _downloader.DownloadProgress += Downloader_DownloadProgress;
            _downloader.DownloadPause += Downloader_DownloadPause;
            _downloader.DownloadResume += Downloader_DownloadResume;
            _downloader.DownloadError += Downloader_DownloadError;
            _downloader.DownloadEnd += Downloader_DownloadEnd;
            _downloader.FollowingHq += Downloader_FollowingHq;
            _downloader.UpdateHq += Downloader_UpdateHq;
        }

        public void Download(ModelBase item, string directory) {
            lock (_lockThis) {
                if (item.GetType() == typeof(Hq)) {
                    var hq = item as Hq;
                    _downloader.SaveHq(hq, directory);
                }
                if (item.GetType() == typeof(Chapter)) {
                    var chapter = item as Chapter;
                    _downloader.SaveChapter(chapter, directory);
                }
            }
        }

        public void PauseResumeDownload(bool state) => _downloader.PauseRemumeDownload(state);

        public void UpdateHqs(List<Hq> hqsToUpdate, string directory) {
            lock (_lockThis2) {
                var startTime = DateTime.Now;
                OnUpdateStart(new UpdateEventArgs(hqsToUpdate, directory, startTime));
                var totalTime = _timerHelper.RuntimeOf(() => {
                    Parallel.ForEach(hqsToUpdate, (hq) => {
                        _downloader.SaveHq(hq, directory);
                    });
                });
                OnUpdateEnd(new UpdateEventArgs(hqsToUpdate, directory, startTime, DateTime.Now, totalTime));
            }
        }

        public void FollowHq(Hq hq, string directory) => _downloader.FollowHq(hq, directory);

        public List<HqDownloadInfo> GetDownloadedHqsInfo() => _downloader.GetDownloadedHqsInfo();

        public HqDownloadInfo GetDownloadedHqInfo(string link) => _downloader.GetDownloadedHqInfo(link);

        private void Downloader_FollowingHq(object sender, FollowEventArgs ev) => OnFollowingHq(ev);

        private void Downloader_UpdateHq(object sender, UpdateEventArgs ev) => OnUpdateHq(ev);

        private void Downloader_DownloadStart(object sender, DownloadEventArgs ev) => OnDownloadStart(ev);

        private void Downloader_DownloadProgress(object sender, ProgressEventArgs ev) => OnDownloadProgress(ev);

        private void Downloader_DownloadPause(object sender, ProgressEventArgs ev) => OnDownloadPause(ev);

        private void Downloader_DownloadResume(object sender, ProgressEventArgs ev) => OnDownloadResume(ev);

        private void Downloader_DownloadError(object sender, DownloadErrorEventArgs ev) => OnDownloadError(ev);

        private void Downloader_DownloadEnd(object sender, DownloadEventArgs ev) => OnDownloadEnd(ev);

        private void OnDownloadStart(DownloadEventArgs e) => DownloadStart?.Invoke(this, e);

        private void OnDownloadProgress(ProgressEventArgs e) => DownloadProgress?.Invoke(this, e);

        private void OnDownloadPause(ProgressEventArgs e) => DownloadPause?.Invoke(this, e);

        private void OnDownloadResume(ProgressEventArgs e) => DownloadResume?.Invoke(this, e);

        private void OnDownloadEnd(DownloadEventArgs e) => DownloadEnd?.Invoke(this, e);

        private void OnDownloadError(DownloadErrorEventArgs e) => DownloadError?.Invoke(this, e);

        private void OnFollowingHq(FollowEventArgs e) => FollowingHq?.Invoke(this, e);

        private void OnUpdateHq(UpdateEventArgs e) => UpdateHq?.Invoke(this, e);

        private void OnUpdateStart(UpdateEventArgs e) => UpdateStart?.Invoke(this, e);

        private void OnUpdateEnd(UpdateEventArgs e) => UpdateEnd?.Invoke(this, e);
    }
}
