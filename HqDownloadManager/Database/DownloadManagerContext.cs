using ADO.ORM;
using ADO.ORM.Core.SqLite;
using HqDownloadManager.Models;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Database {
    public class DownloadManagerContext : SqLiteContext {
        public static string DatabaseLocation { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}\\Databases";
        public DownloadManagerContext() : base(DatabaseLocation, "DownloadManagerContext") {

        }

        public Repository<DownloadedHq> DownloadedHQ { get; set; }
        public Repository<DownloadedChapter> DownloadedChapter { get; set; }
        public Repository<DownloadItem> DownloadList { get; set; }
    }
}
