using HqDownloadManager.Core.Models;
using Repository.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Download.Models {
    public class DownloadItem {
        [PrimaryKey]
        public int Id { get; set; }
        public virtual Hq Hq { get; set; }
        [Required(false)]
        public virtual Chapter LastDownloadedChapter { get; set; }
        [Required(false)]
        public virtual Page ActualPage { get; set; }
        public string DownloadLocation { get; set; }
        public bool IsDownloaded { get; set; }
        public DateTime DownloadStarted { get; set; }
        public DateTime DownloadFinished { get; set; }

        public override bool Equals(object obj) {
            if (obj is DownloadItem downloadItem) {
                return Hq.Link == downloadItem.Hq.Link;
            }
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
