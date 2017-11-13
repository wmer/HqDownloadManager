using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Download.Models
{
    public class HqDownloadInfo {
        [PrimaryKey]
        public string Link { get; set; }
        public string SavedIn { get; set; }
        public DateTime Time { get; set; }
        public byte[] HqDownloaded { get; set; }

        public override bool Equals(object obj) {
            if (obj.GetType() == typeof(HqDownloadInfo)) {
                var model = (HqDownloadInfo)obj;
                return SavedIn == model.SavedIn;
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
