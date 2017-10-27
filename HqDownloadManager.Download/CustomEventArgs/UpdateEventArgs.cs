using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Download.CustomEventArgs {
    public class UpdateEventArgs : EventArgs {
        public List<Hq> HqsToUpdate { get; private set; }
        public String Path { get; private set; }
        public DateTime Time { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TimeSpan TotalTime { get; private set; }

        public UpdateEventArgs(List<Hq> hqsToUpdate, DateTime time) {
            HqsToUpdate = hqsToUpdate;
            Time = time;
        }

        public UpdateEventArgs(List<Hq> hqsToUpdate, String Path, DateTime startTime) {
            HqsToUpdate = hqsToUpdate;
            StartTime = startTime;
        }

        public UpdateEventArgs(List<Hq> hqsToUpdate, String Path, DateTime startTime, DateTime endTime, TimeSpan totalTime) {
            HqsToUpdate = hqsToUpdate;
            StartTime = startTime;
            EndTime = endTime;
            TotalTime = totalTime;
        }
    }
    public delegate void UpdateEventHandler(object sender, UpdateEventArgs ev);
}
