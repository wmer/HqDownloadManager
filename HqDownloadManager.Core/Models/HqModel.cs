using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HqDownloadManager.Core.Models {
    [Table("Hq")]
    public class HqModel {
        [PrimaryKey]
        public string Link { get; set; }
        public DateTime TimeCache { get; set; }
        public byte[] Hq { get; set; }
    }
}