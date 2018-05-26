using ADO.ORM;
using ADO.ORM.Attributes;
using ADO.ORM.Core.SqLite;
using HqDownloadManager.Models;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Database {
    public class DownloadManagerContext : SqLiteContext {
        [Ignore]
        public static string DatabaseLocation { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}\\Databases";
        public DownloadManagerContext() : base(DatabaseLocation, "DownloadManagerContext") {

        }

        public Repository<DownloadedHq> DownloadedHq { get; set; }
        public Repository<DownloadedChapter> DownloadedChapter { get; set; }
        public Repository<DownloadItem> DownloadList { get; set; }
        public Repository<DownloadLocation> DownloadLocation { get; set; }
    }
}
