using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Attributes;

namespace HqDownloadManager.Core.Models {
    public class ModelBase {
        [PrimaryKey]
        public String Link { get; set; }
        public String Title { get; set; }
        public string Path { get; set; }
        public bool Downloaded { get; set; }
        public bool Read { get; set; }
    }
}
