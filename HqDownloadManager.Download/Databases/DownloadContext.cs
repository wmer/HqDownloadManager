using System;
using System.Collections.Generic;
using System.Text;
using HqDownloadManager.Download.Models;
using Repository.Core;
using Repository;

namespace HqDownloadManager.Download.Databases {
    internal class DownloadContext : DBContext {
        public DownloadContext() : this($"{AppDomain.CurrentDomain.BaseDirectory}\\databases") { }

        public DownloadContext(string path) : base($"{path}\\databases", "downloadDb.db") {}

        public Repository<HqDownloadInfo> HqDownloadInfo { get; set; }
    }
}
