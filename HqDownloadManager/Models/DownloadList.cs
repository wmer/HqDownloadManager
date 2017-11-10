using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Attributes;

namespace HqDownloadManager.Models {
    public class DownloadList {
        [PrimaryKey]
        public int Id { get; set; }
        public byte[] List { get; set; }
    }
}
