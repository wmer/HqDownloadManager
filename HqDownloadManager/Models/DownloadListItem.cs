using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Models {
    public class DownloadListItem : IComparable<DownloadListItem> {
        public Hq Hq { get; set; }
        public string Status { get; set; }

        public int CompareTo(DownloadListItem other) {
            return String.Compare(Hq.Link, other.Hq.Link, StringComparison.Ordinal);
        }
    }
}
