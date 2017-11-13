using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.Models {
    public class UserPreferences {
        [PrimaryKey]
        public int Id { get; set; }
        public byte[] UserPreferencesViewModel { get; set; }
    }
}
