using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.Database;
using HqManager.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqManager.Database {
    public class HqManagerContext : LibraryContext {
        public Repository<HqEntry> HqEntry { get; set; }
    }
}
