using System;
using System.Collections.Generic;
using System.Text;
using Repository.Attributes;

namespace HqDownloadManager.Follow.Models {
    public class FollowedHq {
        [PrimaryKey]
        public string Link { get; set; }
        public DateTime Time { get; set; }
        public byte[] Hq { get; set; }
    }
}
