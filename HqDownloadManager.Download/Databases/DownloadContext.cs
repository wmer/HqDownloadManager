using System;
using System.Collections.Generic;
using System.Text;
using HqDownloadManager.Download.Models;
using Repository.Core;
using Repository;

namespace HqDownloadManager.Download.Databases {
    internal class DownloadContext : DBContext {
        public DownloadContext() : base($"{AppDomain.CurrentDomain.BaseDirectory}\\databases", "downloadDb.db") { }

        public Repository<HqDownloadInfo> HqDownloadInfo { get; set; }
    }
}
