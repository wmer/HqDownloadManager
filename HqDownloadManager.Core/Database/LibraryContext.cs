using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.Models;
using Repository;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Database {
    internal class LibraryContext : DBContext {
        public LibraryContext() : base($"{CoreConfiguration.DatabaseLocation}", "libraryCache.db") {
        }

        public Repository<Cache> Cache { get; set; }
    }
}
