using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.FollowUpdate.CustomEventArgs
{
    public class UpdateEventArgs : EventArgs {
        public Hq HqToUpdate { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TimeSpan TotalTime { get; private set; }

        public UpdateEventArgs(Hq hqToUpdate, DateTime startTime) {
            HqToUpdate = hqToUpdate;
            StartTime = startTime;
        }

        public UpdateEventArgs(Hq hqToUpdate, DateTime startTime, DateTime endTime, TimeSpan totalTime) {
            HqToUpdate = hqToUpdate;
            StartTime = startTime;
            EndTime = endTime;
            TotalTime = totalTime;
        }
    }
    public delegate void UpdateEventHandler(object sender, UpdateEventArgs ev);
}
