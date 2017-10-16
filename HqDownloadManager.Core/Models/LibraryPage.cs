using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Models {
    public class LibraryPage {
        public String FinalizedPage { get; set; }
        public String FirstPage { get; set; }
        public String ActualPage { get; set; }
        public String NextPage { get; set; }
        public String LastPage { get; set; }
        public IEnumerable<Hq> Hqs { get; set; }

        public override bool Equals(object obj) {
            if (obj.GetType() == typeof(LibraryPage)) {
                var model = (LibraryPage)obj;
                return ActualPage == model.ActualPage;
            }
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
