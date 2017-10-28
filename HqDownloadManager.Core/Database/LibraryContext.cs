using HqDownloadManager.Core.Models;
using Repository;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Database {
    internal class LibraryContext : DBContext {
        public LibraryContext() : base($"{AppDomain.CurrentDomain.BaseDirectory}\\databases", "libraryCache.db") {
        }
        public LibraryContext(string path, string dbName) : base(path, dbName) {
        }

        public Repository<Cache> Cache { get; set; }
    }
}
