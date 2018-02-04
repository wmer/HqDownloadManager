using HqDownloadManager.Core.Models;
using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqManager.Models {
    public class HqEntry {
        [PrimaryKey]
        public virtual Hq Hq { get; set; }
        public string ReadStatus { get; set; }
        [Required(false)]
        public virtual Chapter LastChapterRead { get; set; }
        public double Score { get; set; }
        public string Review { get; set; }
        public DateTime StartedReading { get; set; }
        public DateTime FinishedReading { get; set; }
    }
}
