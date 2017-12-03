using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.Models {
    public class UserReading {
        [PrimaryKey]
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public byte[] Reading { get; set; }
    }
}
