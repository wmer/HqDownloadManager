using ADO.ORM.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Models {
    public class Download {
        [PrimaryKey]
        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
    }
}
