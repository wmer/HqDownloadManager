using HqDownloadManager.Models;
using Repository;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Database {
    public class UserLibraryContext : DBContext {
        public UserLibraryContext() : base($"{AppDomain.CurrentDomain.BaseDirectory}/databases", "userLibrary.db") { }

        public Repository<User> Users { get; set; }
        public Repository<UserPreferences> UserPreferences { get; set; }
        public Repository<UserReading> UserReadings { get; set; }
        public Repository<UserFavorite> UserFavorites { get; set; }
        public Repository<UserDownload> UserDownloads { get; set; }
    }
}

