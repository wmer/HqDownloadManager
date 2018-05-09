using ADO.ORM.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Models {
    public class DownloadedChapter : Download {
        public virtual DownloadedHq DownloadedHq { get; set; }
    }
}
