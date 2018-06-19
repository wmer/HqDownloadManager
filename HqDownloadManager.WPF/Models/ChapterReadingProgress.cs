using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.WPF.Models {
    public class ChapterReadingProgress {
        public int Id { get; set; }
        public string ChapterTitle { get; set; }
        public int ChapterIndex { get; set; }
        public string HqLocation { get; set; }
        public string Location { get; set; }
        public int TotalPages { get; set; }
        public int ActualPage { get; set; }
        public string LastPageLocation { get; set; }
        public DateTime Date { get; set; }
    }
}
