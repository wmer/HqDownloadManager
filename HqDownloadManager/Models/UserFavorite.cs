using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Attributes;

namespace HqDownloadManager.Models {
    public class UserFavorite {
        [PrimaryKey]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public byte[] Favorite { get; set; }
    }
}
