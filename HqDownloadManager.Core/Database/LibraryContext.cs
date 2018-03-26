using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.Models;
using Repository;
using Repository.Core.MySql;
using Repository.Core.SqLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Database {
    public class LibraryContext : MySqlContext {
        public LibraryContext() : base("localhost", 3306, "library", "root", "EWSantanas3120") {
        }

        public Repository<Cache> Cache { get; set; }
        public Repository<Update> Update { get; set; }
        public Repository<Hq> Hq { get; set; }
        public Repository<Chapter> Chapter { get; set; }
        public Repository<Page> Page { get; set; }
    }
}
