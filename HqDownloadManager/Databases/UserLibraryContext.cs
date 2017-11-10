using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using HqDownloadManager.Models;
using Repository.Core;
using Repository;

namespace HqDownloadManager.Databases {
    public class UserLibraryContext : DBContext {
        public UserLibraryContext() : base($"{ApplicationData.Current.LocalFolder.Path}\\databases", "userLibrary.db") { }

        public Repository<UserPreferences> UserPreferences { get; set; }
        public Repository<UserReading> UserReadings { get; set; }
        public Repository<UserFavorite> UserFavorites { get; set; }
        public Repository<DownloadList> DownloadList { get; set; }
    }
}
