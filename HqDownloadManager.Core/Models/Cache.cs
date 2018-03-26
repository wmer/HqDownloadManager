using SqlCreator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Models
{
    public class Cache {
        [PrimaryKey]
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public byte[] ModelsCache { get; set; }
    }
}
