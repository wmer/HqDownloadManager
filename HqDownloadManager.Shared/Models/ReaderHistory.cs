using HqDownloadManager.Shared.ViewModel.Reader;
using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.Models {
    public class ReaderHistory {
        [PrimaryKey]
        public int Id { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public virtual ReaderViewModel Reader { get; set; }
    }
}
