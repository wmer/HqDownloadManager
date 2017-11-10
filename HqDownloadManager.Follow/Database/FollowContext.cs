using System;
using System.Collections.Generic;
using System.Text;
using HqDownloadManager.Follow.Models;
using Repository;
using Repository.Core;

namespace HqDownloadManager.Follow.Database {
    internal class FollowContext : DBContext {
        public FollowContext() : this($"{AppDomain.CurrentDomain.BaseDirectory}\\databases") { }

        public FollowContext(string path) : base($"{path}\\databases", "FollowedDb.db")
        {
            
        }

        public Repository<FollowedHq> FollowedHq { get; set; }
    }
}
