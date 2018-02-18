using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.Models;
using Repository;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Database {
    public class LibraryContext : DBContext {
        public LibraryContext() : base($"{CoreConfiguration.DatabaseLocation}", "libraryCache.db") {
        }

        public Repository<Cache> Cache { get; set; }
        public Repository<Update> Update { get; set; }
        public Repository<Hq> Hq { get; set; }
        public Repository<Chapter> Chapter { get; set; }
        public Repository<Page> Page { get; set; }
    }
}
