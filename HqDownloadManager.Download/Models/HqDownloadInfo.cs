using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Attributes;

namespace HqDownloadManager.Download.Models {
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
