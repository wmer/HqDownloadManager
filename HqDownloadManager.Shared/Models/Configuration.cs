using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.Models {
    public class Configuration {
        [PrimaryKey]
        public int Id { get; set; }
        public string DatabaseLocation { get; set; }
        public string CacheLocation { get; set; }
        public string WebdriverLocation { get; set; }
        public virtual List<DownloadLocation> DownloadLocations { get; set; }
    }
}
