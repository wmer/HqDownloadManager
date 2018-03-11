using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.Database;
using HqDownloadManager.Download.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Download.Databases {
    public class DownloadContext : LibraryContext {
        public Repository<DownloadItem> DownloadList { get; set; }
    }
}
