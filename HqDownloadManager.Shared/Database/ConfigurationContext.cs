using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.Reader;
using Repository;
using Repository.SQLite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.Database {
    public class ConfigurationContext : DBContext {
        public ConfigurationContext() : base($"{AppDomain.CurrentDomain.BaseDirectory}\\Configuration", "configuartion.db") {
        }

        public Repository<DownloadLocation> DownloadLocation { get; set; }
        public Repository<Configuration> Configuration { get; set; }
    }
}
