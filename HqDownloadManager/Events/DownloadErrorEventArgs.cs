using HqDownloadManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Events {
    public class DownloadErrorEventArgs : EventArgs {
        public DownloadItem Item { get; private set; }
        public Exception Cause { get; private set; }
        public DateTime Time { get; private set; }

        public DownloadErrorEventArgs(DownloadItem item, Exception cause, DateTime dateTime) {
            Item = item;
            Cause = cause;
            Time = dateTime;
        }
    }

    public delegate void DownloadErrorEventHandler(object sender, DownloadErrorEventArgs ev);
}
