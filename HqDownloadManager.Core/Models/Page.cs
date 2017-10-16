using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Attributes;

namespace HqDownloadManager.Core.Models {
    public class Page {
        public int Number { get; set; }
        [PrimaryKey]
        public String Source { get; set; }
        public String LocalSource { get; set; }
        public virtual Chapter Chapter { get; set; }
    }
}
