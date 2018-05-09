using HqDownloadManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Events {
    public class DownloadEventArgs : EventArgs {
        public DownloadItem Item { get; private set; }
        public TimeSpan TotalTime { get; private set; }
        public List<String> FailedToDownload { get; private set; }

        public DownloadEventArgs(DownloadItem item) {
            Item = item;
        }

        public DownloadEventArgs(DownloadItem item, TimeSpan totalTime) {
            Item = item;
            TotalTime = totalTime;
        }

        public DownloadEventArgs(DownloadItem item, TimeSpan totalTime, List<String> failedToDownload) {
            Item = item;
            TotalTime = totalTime;
            FailedToDownload = failedToDownload;
        }
    }

    public delegate void DownloadEventHandler(object sender, DownloadEventArgs ev);
}
