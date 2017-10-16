using HqDownloadManager.Core.Models;
using Repository;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager {
    public class DatabaseContext : DBContext {
        public DatabaseContext() : base($"{AppDomain.CurrentDomain.BaseDirectory}/databases", "library.db") {
        }
        public DatabaseContext(string path, string dbName) : base(path, dbName) {
        }

        public Repository<Hq> Hqs { get; set; }
        public Repository<Chapter> Chapters { get; set; }
        public Repository<Page> Pages { get; set; }
    }
}
