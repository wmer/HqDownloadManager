using HqDownloadManager.Core.Models;
using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Models {
    public class UserDownload {
        [PrimaryKey]
        public int Id { get; set; }
        public virtual Hq Hq { get; set; }
        public virtual Chapter Chapter { get; set; }
        public String DownloadPath { get; set; }
        public DateTime Date { get; set; }
    }
}
