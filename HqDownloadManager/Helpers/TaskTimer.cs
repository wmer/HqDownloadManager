using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HqDownloadManager.Helpers {
    public class TaskTimer {
        public TimeSpan RuntimeOf(Action method) {
            var watch = Stopwatch.StartNew();
            method();
            watch.Stop();
            var elapsedTime = watch.Elapsed;
            return elapsedTime;
        }
    }
}
