using ADO.ORM.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Models {
    public class DownloadItem {
        [PrimaryKey]
        public int Id { get; set; }
        public byte[] Hq { get; set; }
        public string DownloadLocation { get; set; }
        public bool IsDownloaded { get; set; }
        public DateTime DownloadStarted { get; set; }
        public DateTime DownloadFinished { get; set; }
    }
}
