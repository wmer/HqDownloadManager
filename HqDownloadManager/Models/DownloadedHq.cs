using ADO.ORM.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Models {
    public class DownloadedHq : Download {
        public virtual List<DownloadedChapter> Chapters { get; set; }
    }
}
