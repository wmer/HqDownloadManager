using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HqDownloadManager.Compression.CustomEventArgs {
    public class CompressionEventArgs : EventArgs {
        public DirectoryInfo Path { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TimeSpan TotalTime { get; private set; }

        public CompressionEventArgs(DirectoryInfo path, DateTime startTime) {
            Path = path;
            StartTime = startTime;
        }

        public CompressionEventArgs(DirectoryInfo path, DateTime startTime, DateTime endTime) {
            Path = path;
            StartTime = startTime;
            EndTime = endTime;
        }

        public CompressionEventArgs(DirectoryInfo path, DateTime startTime, DateTime endTime, TimeSpan totalTime) {
            Path = path;
            StartTime = startTime;
            EndTime = endTime;
            TotalTime = totalTime;
        }
    }

    public delegate void CompressionEventHandler(object sender, CompressionEventArgs ev);
}
