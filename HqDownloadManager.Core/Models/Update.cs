using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Models {
    public class Update {
        public Hq Hq { get; set; }
        public List<Chapter> Chapters { get; set; }

        public override bool Equals(object obj) {
            if (obj.GetType() == typeof(Update)) {
                var model = (Update)obj;
                return Hq.Title == model.Hq.Title;
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
