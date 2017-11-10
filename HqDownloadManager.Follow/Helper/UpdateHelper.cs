using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Follow.CustomEventArgs;
using HqDownloadManager.Follow.Database;
using HqDownloadManager.Follow.Helpers;
using HqDownloadManager.Follow.Models;
using HqDownloadManager.Utils;
using Newtonsoft.Json;

namespace HqDownloadManager.Follow.Helper {
    internal class UpdateHelper {
        private FollowContext _followContext;
        private SourceManager _sourceManager;
        private FollowHelper _followHelper;
        private TaskTimer _taskTimer;
        
        public event UpdateEventHandler UpdateStart;
        public event UpdateEventHandler UpdateEnd;

        private readonly object _lock1 = new object();

        public UpdateHelper(FollowContext followContext, SourceManager sourceManager, FollowHelper followHelper, TaskTimer taskTimer) {
            _followContext = followContext;
            _sourceManager = sourceManager;
            _followHelper = followHelper;
            _taskTimer = taskTimer;
        }

        public List<Chapter> GetUpdates(string hqLink) {
            lock (_lock1) {
                var update = new List<Chapter>();
                Hq hq = null;
                var startTime = DateTime.Now;
                var tototalTime = _taskTimer.RuntimeOf(() => {
                    if (!(_followContext.FollowedHq.FindOne(hqLink) is FollowedHq followedHq)) return;
                    hq = followedHq.Hq.ToObject<Hq>();
                    UpdateStart(this, new UpdateEventArgs(hq, startTime));
                    if (!(_sourceManager.GetInfo(hqLink) is Hq hqInfo)) return;
                    update.AddRange(hqInfo.Chapters.Where(chap => !hq.Chapters.Contains(chap)));
                    if (update.Count > 0) {
                        _followHelper.FollowHq(hqInfo);
                    }
                });


                UpdateEnd(this, new UpdateEventArgs(hq, startTime, DateTime.Now, tototalTime));
                return update;
            }
        }

        
    }
}
