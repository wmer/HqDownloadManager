using HqDownloadManager.FollowUpdate.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.FollowUpdate.Helpers {
    public static class FollowUpdateEventHub {
        public static event UpdateEventHandler UpdateStart;
        public static event UpdateEventHandler UpdateEnd;
        public static event FollowEventHandler FollowingHq;


        public static void OnUpdateStart(object sender, UpdateEventArgs ev) => UpdateStart?.Invoke(sender, ev);
        public static void OnUpdateEnd(object sender, UpdateEventArgs ev) => UpdateEnd?.Invoke(sender, ev);
        public static void OnFollowingHq(object sender, FollowEventArgs ev) => FollowingHq?.Invoke(sender, ev);
    }
}
