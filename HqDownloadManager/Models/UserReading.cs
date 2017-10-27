using HqDownloadManager.Core.Models;
using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Models {
    public class UserReading {
        [PrimaryKey]
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public byte[] HqReaderViewModel { get; set; }
    }
}
