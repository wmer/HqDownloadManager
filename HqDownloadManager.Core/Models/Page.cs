using SqlCreator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Models {
    public class Page {
        [PrimaryKey]
        public int Id { get; set; }
        public String Source { get; set; }
        public int Number { get; set; }
        public virtual Chapter Chapter { get; set; }
    }
}
