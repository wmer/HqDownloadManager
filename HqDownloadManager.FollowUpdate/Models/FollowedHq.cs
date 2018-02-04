using HqDownloadManager.Core.Models;
using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.FollowUpdate.Models
{
    public class FollowedHq {
        [PrimaryKey]
        public virtual Hq Hq { get; set; }
        public DateTime Time { get; set; }
    }
}
