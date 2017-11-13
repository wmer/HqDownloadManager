using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.FollowUpdate.Models
{
    public class FollowedHq {
        [PrimaryKey]
        public string Link { get; set; }
        public DateTime Time { get; set; }
        public byte[] Hq { get; set; }
    }
}
