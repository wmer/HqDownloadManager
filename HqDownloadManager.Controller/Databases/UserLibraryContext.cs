using HqDownloadManager.Controller.Models;
using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Core.Database;
using Repository;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HqDownloadManager.Controller.Databases {
    public class UserLibraryContext : LibraryContext {
        public Repository<UserPreferences> UserPreferences { get; set; }
        public Repository<UserReading> UserReadings { get; set; }
        public Repository<DownloadList> DownloadList { get; set; }
    }
}
