using HqDownloadManager.Core.Models;
using Repository;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Database {
    internal class LibraryContext : DBContext {
        public LibraryContext() : base($"{AppDomain.CurrentDomain.BaseDirectory}/databases", "library.db") {
        }
        public LibraryContext(string path, string dbName) : base(path, dbName) {
        }

        public Repository<Hq> Hqs { get; set; }
        public Repository<Chapter> Chapters { get; set; }
        public Repository<Page> Pages { get; set; }
    }
}
