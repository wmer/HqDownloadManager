using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.FollowUpdate.CustomEventArgs;
using HqDownloadManager.FollowUpdate.Databases;
using HqDownloadManager.FollowUpdate.Models;
using Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.FollowUpdate.Helpers {
    internal class FollowHelper {
        private readonly FollowContext _context;
        private readonly SourceManager _sourceManager;

        public FollowHelper(FollowContext context, SourceManager sourceManager) {
            this._context = context;
            this._sourceManager = sourceManager;
        }

        private readonly object _lock1 = new object();
        private readonly object _lock2 = new object();
        private readonly object _lock3 = new object();

        public void FollowHq(Hq hq) {
            lock (_lock1) {
                _context.Hq.Update(x => new { x.Followed }, true)
                                                         .Where(x => x.Link == hq.Link).Execute();
                FollowUpdateEventHub.OnFollowingHq(this, new FollowEventArgs(hq, DateTime.Now));
            }
        }

        public Hq GetFollowedHq(string link) => _context.Hq.FindOne(link);

        public List<Hq> GetAllFollowedHqs() =>
                     _context.Hq.Find().Where(x => x.Followed == true).Execute();
    }
}
