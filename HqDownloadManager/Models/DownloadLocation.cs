using ADO.ORM.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Models {
    public class DownloadLocation {
        [PrimaryKey]
        public int Id { get; set; }
        public string Location { get; set; }
    }
}
