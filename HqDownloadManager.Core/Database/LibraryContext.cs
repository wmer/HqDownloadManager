using HqDownloadManager.Core.Models;
using Repository;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Database {
    internal class LibraryContext : DBContext {
        public LibraryContext() : this($"{AppDomain.CurrentDomain.BaseDirectory}\\databases") {
        }
        public LibraryContext(string path) : base($"{path}\\databases", "libraryCache.db") {
        }

        public Repository<Cache> Cache { get; set; }
    }
}
