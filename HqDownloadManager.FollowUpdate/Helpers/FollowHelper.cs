using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.FollowUpdate.CustomEventArgs;
using HqDownloadManager.FollowUpdate.Databases;
using HqDownloadManager.FollowUpdate.Models;
using HqDownloadManager.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.FollowUpdate.Helpers
{
    internal class FollowHelper {
        private readonly FollowContext _context;
        private readonly SourceManager _sourceManager;

        public event FollowEventHandler FollowingHq;

        public FollowHelper(FollowContext context, SourceManager sourceManager) {
            this._context = context;
            this._sourceManager = sourceManager;
        }

        private readonly object _lock1 = new object();
        private readonly object _lock2 = new object();
        private readonly object _lock3 = new object();

        public async Task FollowHq(Hq hq) {
            if (!(await _sourceManager.GetInfo(hq.Link) is Hq hqInfo)) return;
            var dInfo = new FollowedHq {
                Link = hq.Link,
                Time = DateTime.Now,
                Hq = hqInfo.ToBytes()
            };
            if (_context.FollowedHq.FindOne(hq.Link) != null) {
                _context.FollowedHq.Update(dInfo);
            } else {
                _context.FollowedHq.Save(dInfo);
            }
            FollowingHq(this, new FollowEventArgs(hqInfo, DateTime.Now));
        }

        public FollowedHq GetFollowedHq(string link) {
            lock (_lock2) {
                return _context.FollowedHq.FindOne(link);
            }
        }

        public List<FollowedHq> GetAllFollowedHqs() {
            lock (_lock3) {
                return _context.FollowedHq.FindAll();
            }
        }
    }
}
