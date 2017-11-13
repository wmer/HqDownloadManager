using HqDownloadManager.Core.Configuration;
using HqDownloadManager.FollowUpdate.Models;
using Repository;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.FollowUpdate.Databases
{
    internal class FollowContext : DBContext {
        public FollowContext() : base($"{CoreConfiguration.DatabaseLocation}", "FollowedDb.db") { }

        public Repository<FollowedHq> FollowedHq { get; set; }
    }
}
