using Repository.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.Models {
    public class DownloadLocation {
        [PrimaryKey]
        public int Id { get; set; }
        public string Location { get; set; }
    }
}
