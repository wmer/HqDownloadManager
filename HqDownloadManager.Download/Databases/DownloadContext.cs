using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Download.Models;
using Repository;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Download.Databases
{
    internal class DownloadContext : DBContext {
        public DownloadContext() : base($"{CoreConfiguration.DatabaseLocation}", "downloadDb.db") { }        

        public Repository<HqDownloadInfo> HqDownloadInfo { get; set; }
    }
}
