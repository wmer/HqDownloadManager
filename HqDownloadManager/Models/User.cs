using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Models {
    public class User {
        [PrimaryKey]
        public int Id { get; set; }
        public String Name { get; set; }
        public String UserName { get; set; }
        public String Senha { get; set; }
        [Required(false)]
        public virtual UserPreferences UserPreferences { get; set; }
        public virtual List<UserFavorite> Favorites { get; set; }
        public virtual List<UserReading> ReadingList { get; set; }
        public virtual List<UserDownload> Downloads { get; set; }
    }
}
