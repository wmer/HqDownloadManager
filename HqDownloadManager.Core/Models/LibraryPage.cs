using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Models {
    public class LibraryPage {
        public string FinalizedPage { get; set; }
        public string NextPage { get; set; }
        public List<Hq> Hqs { get; set; }
        public Dictionary<string, string> Letras { get; set; }
    }
}
