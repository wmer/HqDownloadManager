using HqDownloadManager.Controller.ViewModel;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.Models {
    public class DownloadListItem : IComparable<DownloadListItem> {
        public Hq Hq { get; set; }
        public Chapter DownloadChapter { get; set; }
        public int TotalChapters { get; set; }
        public int ActualChapter { get; set; }
        public int TotalPages { get; set; }
        public int ActualPage { get; set; }
        public DateTime StartedIn { get; set; }
        public DateTime FinishedIn { get; set; }
        public string Status { get; set; }


        public int CompareTo(DownloadListItem other) {
            return String.Compare(Hq.Link, other.Hq.Link, StringComparison.Ordinal);
        }
    }
}
