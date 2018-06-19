using ADO.ORM.Attributes;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Models {
    public class DownloadedHq : Download {
        [Ignore]
        public Hq Hq { get; set; }
        public virtual List<DownloadedChapter> Chapters { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as DownloadedHq);
        }

    }
}
