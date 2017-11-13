using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HqDownloadManager.Download.CustomEventArgs {
    public class DownloadEventArgs : EventArgs {
        public ModelBase Item { get; private set; }
        public DirectoryInfo Path { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TimeSpan TotalTime { get; private set; }
        public List<String> FailedToDownload { get; private set; }

        public DownloadEventArgs(ModelBase item, DirectoryInfo path, DateTime startTime) {
            Item = item;
            Path = path;
            StartTime = startTime;
        }

        public DownloadEventArgs(ModelBase item, DirectoryInfo path, DateTime startTime, DateTime endTime, TimeSpan totalTime) {
            Item = item;
            Path = path;
            StartTime = startTime;
            EndTime = endTime;
            TotalTime = totalTime;
        }

        public DownloadEventArgs(ModelBase item, DirectoryInfo path, DateTime startTime, DateTime endTime, TimeSpan totalTime, List<String> failedToDownload) {
            Item = item;
            Path = path;
            StartTime = startTime;
            EndTime = endTime;
            TotalTime = totalTime;
            FailedToDownload = failedToDownload;
        }
    }

    public delegate void DownloadEventHandler(object sender, DownloadEventArgs ev);
}
