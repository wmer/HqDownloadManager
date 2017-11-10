using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Follow.CustomEventArgs {
    public class FollowEventArgs : EventArgs {
        public Hq Hq { get; private set; }
        public DateTime Time { get; private set; }

        public FollowEventArgs(Hq hq, DateTime time) {
            Hq = hq;
            Time = time;
        }
    }

    public delegate void FollowEventHandler(object sender, FollowEventArgs ev);
}
