using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Models {
    public class Update {
        public Hq Hq { get; set; }
        public List<Chapter> Chapters { get; set; }
    }
}
