using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Models {
    internal class Cache {
        [PrimaryKey]
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public byte[] ModelsCache { get; set; }
    }
}
