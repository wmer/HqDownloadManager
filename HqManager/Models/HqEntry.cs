using HqDownloadManager.Core.Models;
using Repository.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqManager.Models {
    public class HqEntry {
        [PrimaryKey]
        public virtual Hq Hq { get; set; }
        public string ReadStatus { get; set; }
        public string LastChapterRead { get; set; }
        public double Score { get; set; }
        public string Review { get; set; }
        public DateTime StartedReading { get; set; }
        public DateTime LastChapterReadDate { get; set; }
        public DateTime FinishedReading { get; set; }
    }
}
