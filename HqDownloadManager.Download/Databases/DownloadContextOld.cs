using System;
using System.Collections.Generic;
using System.Text;
using HqDownloadManager.Download.Models;
using Repository.Core;
using Repository;
using HqDownloadManager.Core.Configuration;

namespace HqDownloadManager.Download.Databases {
    public class DownloadContextOld : DBContext {
        public DownloadContextOld() : base($"{CoreConfiguration.DatabaseLocation}", "downloadDb.db") { }
        

        public Repository<HqDownloadInfo> HqDownloadInfo { get; set; }
    }
}
