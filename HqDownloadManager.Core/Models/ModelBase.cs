using SqlCreator.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Models {
    public class ModelBase {
        [PrimaryKey]
        public int Id { get; set; }
        public String Link { get; set; }
        public String Title { get; set; }
        public DateTime TimeInCache { get; set; }
    }
}
