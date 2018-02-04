using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Models {
    public class Hq : ModelBase, IComparable<Hq> {
        public string CoverSource { get; set; }
        public string Author { get; set; }
        public string Synopsis { get; set; }
        public bool IsFinalized { get; set; }
        public bool IsDetailedInformation { get; set; }
        public virtual List<Chapter> Chapters { get; set; }

        public int CompareTo(Hq other) {
            return String.Compare(Title, other.Title, StringComparison.Ordinal);
        }

        public override bool Equals(object obj) {
            if (obj.GetType() == typeof(Hq)) {
                var model = (Hq)obj;
                return Link == model.Link || Title == model.Title;
            }
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
