using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.FollowUpdate.CustomEventArgs;
using HqDownloadManager.FollowUpdate.Databases;
using HqDownloadManager.FollowUpdate.Models;
using Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.FollowUpdate.Helpers {
    internal class UpdateHelper {
        private FollowContext _followContext;
        private SourceManager _sourceManager;
        private FollowHelper _followHelper;
        private TaskTimer _taskTimer;
        
        public UpdateHelper(FollowContext followContext, SourceManager sourceManager, FollowHelper followHelper, TaskTimer taskTimer) {
            _followContext = followContext;
            _sourceManager = sourceManager;
            _followHelper = followHelper;
            _taskTimer = taskTimer;
        }

        public List<Chapter> GetUpdates(string hqLink) {
            var update = new List<Chapter>();
            Hq hq = null;
            var startTime = DateTime.Now;
            if (!(_followContext.Hq.FindOne(hqLink) is Hq followedHq) || !followedHq.Followed) return update;
            hq = followedHq;
            FollowUpdateEventHub.OnUpdateStart(this, new UpdateEventArgs(hq, startTime));
            var hqInfo = _sourceManager.GetInfo<Hq>(hqLink, false, -1);
            foreach (var chap in hqInfo.Chapters) {
                if (!hq.Chapters.Contains(chap)) {
                    update.Add(chap);
                }
            }

            return update;
        }
    }
}
