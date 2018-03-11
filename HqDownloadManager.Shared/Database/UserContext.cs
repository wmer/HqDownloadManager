using HqDownloadManager.Core.Database;
using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.Reader;
using HqManager.Database;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.Database {
    public class UserContext : HqManagerContext {
        public Repository<ReaderViewModel> Reader { get; set; }
        public Repository<ReadingHistory> ReaderHistory { get; set; }
    }
}
