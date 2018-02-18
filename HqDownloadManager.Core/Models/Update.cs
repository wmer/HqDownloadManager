using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Models {
    public class Update {
        [PrimaryKey]
        public virtual Hq Hq { get; set; }
        public string Source { get; set; }
        public DateTime TimeCache { get; set; }
        public virtual List<Chapter> Chapters { get; set; }

        public override bool Equals(object obj) {
            if (obj.GetType() == typeof(Update)) {
                var model = (Update)obj;
                if (Hq != null && model.Hq != null) {
                    return Hq.Title == model.Hq.Title;
                }
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
