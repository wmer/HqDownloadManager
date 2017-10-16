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
        public virtual List<Chapter> Chapters { get; set; }

        public int CompareTo(Hq other) {
            return Chapters.Count().CompareTo(other.Chapters.Count());
        }
    }
}
