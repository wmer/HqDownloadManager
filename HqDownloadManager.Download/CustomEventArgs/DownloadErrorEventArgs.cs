using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Download.CustomEventArgs
{
    public class DownloadErrorEventArgs : EventArgs {
        public ModelBase Item { get; private set; }
        public Exception Cause { get; private set; }
        public DateTime Time { get; private set; }

        public DownloadErrorEventArgs(ModelBase item, Exception cause, DateTime dateTime) {
            Item = item;
            Cause = cause;
            Time = dateTime;
        }
    }

    public delegate void DownloadErrorEventHandler(object sender, DownloadErrorEventArgs ev);
}
