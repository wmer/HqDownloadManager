using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Attributes;

namespace HqDownloadManager.Models {
    public class UserPreferences {
        [PrimaryKey]
        public int Id { get; set; }
        public byte[] UserPreferencesViewModel { get; set; }
    }
}
