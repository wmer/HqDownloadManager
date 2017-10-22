using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Models {
    public class UserPreferences {
        [PrimaryKey]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public bool Compress { get; set; }
        public bool EraseFolder { get; set; }
        public String DownloadPath { get; set; }
    }
}
