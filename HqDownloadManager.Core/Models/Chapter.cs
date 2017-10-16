using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Models {
    public class Chapter : ModelBase, IComparable<Chapter> {
        public virtual Hq Hq { get; set; }
        public virtual List<Page> Pages { get; set; }

        public int CompareTo(Chapter other) {
            return Pages.Count().CompareTo(other.Pages.Count());
        }

        public override bool Equals(object obj) {
            if (obj.GetType() == typeof(Chapter)) {
                var model = (Chapter)obj;
                return Link == model.Link;
            }
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
