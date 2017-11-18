using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HqDownloadManager.Compression.CustomEventArgs
{
    public class CompressionErrorEventArgs : EventArgs {
        public DirectoryInfo Path { get; private set; }
        public Exception Cause { get; private set; }
        public DateTime Time { get; private set; }

        public CompressionErrorEventArgs(DirectoryInfo path, Exception cause, DateTime dateTime) {
            Path = path;
            Cause = cause;
            Time = dateTime;
        }
    }

    public delegate void CompressionErrorEventHandler(object sender, CompressionErrorEventArgs ev);
}
