using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Follow.CustomEventArgs;
using HqDownloadManager.Follow.Database;
using HqDownloadManager.Follow.Models;
using HqDownloadManager.Utils;
using Newtonsoft.Json;

namespace HqDownloadManager.Follow.Helpers {
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

        public void FollowHq(Hq hq) {
            lock (_lock1) {
                if (!(_sourceManager.GetInfo(hq.Link) is Hq hqInfo)) return;
                var dInfo = new FollowedHq {
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
