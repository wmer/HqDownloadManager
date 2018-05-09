using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Events {
    public static class DownloadEventHub {
        public static event DownloadEventHandler DownloadStart;
        public static event DownloadEventHandler DownloadEnd;
        public static event ProgressEventHandler DownloadProgress;
        public static event ProgressEventHandler DownloadChapterStart;
        public static event DownloadEventHandler DownloadChapterEnd;
        public static event ProgressEventHandler DownloadPause;
        public static event ProgressEventHandler DownloadResume;
        public static event DownloadEventHandler DownloadStop;
        public static event DownloadErrorEventHandler DownloadError;


        public static void OnDownloadStart(object sender, DownloadEventArgs ev) => DownloadStart?.Invoke(sender, ev);

        public static void OnDownloadChapterStart(object sender, ProgressEventArgs ev) => DownloadChapterStart?.Invoke(sender, ev);

        public static void OnDownloadProgress(object sender, ProgressEventArgs ev) => DownloadProgress?.Invoke(sender, ev);

        public static void OnDownloadPause(object sender, ProgressEventArgs ev) => DownloadPause?.Invoke(sender, ev);

        public static void OnDownloadResume(object sender, ProgressEventArgs ev) => DownloadResume?.Invoke(sender, ev);

        public static void OnDownloadStop(object sender, DownloadEventArgs ev) => DownloadStop?.Invoke(sender, ev);

        public static void OnDownloadError(object sender, DownloadErrorEventArgs ev) => DownloadError?.Invoke(sender, ev);

        public static void OnDownloadEnd(object sender, DownloadEventArgs ev) => DownloadEnd?.Invoke(sender, ev);

        public static void OnDownloadChapterEnd(object sender, DownloadEventArgs ev) => DownloadChapterEnd?.Invoke(sender, ev);
    }
}
