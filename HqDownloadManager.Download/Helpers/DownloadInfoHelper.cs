using HqDownloadManager.Core.Database;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Download.Databases;
using HqDownloadManager.Download.Models;
using Newtonsoft.Json;

namespace HqDownloadManager.Download.Helpers {
    internal class DownloadInfoHelper {
        private readonly DownloadContext _downloadContext;
        private readonly object _lock = new object();
        private readonly object _lock2 = new object();


        public DownloadInfoHelper(DownloadContext downloadContext) {
            _downloadContext = downloadContext;
        }

        public void SaveHqDownloadInfo(Hq hq, string path, DateTime time) {
            lock (_lock) {
                var dInfo = new HqDownloadInfo {
                    SavedIn = path, Time = time,
                    HqDownloaded = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(hq))
                };
                if (_downloadContext.HqDownloadInfo.Find().Where(x => x.SavedIn == path).Execute().FirstOrDefault() != null) {
                    _downloadContext.HqDownloadInfo.Update(dInfo);
                } else {
                    _downloadContext.HqDownloadInfo.Save(dInfo);
                }
            }
        }

        public List<HqDownloadInfo> GetHqsDownloadInfo() {
            lock (_lock2) {
                return _downloadContext.HqDownloadInfo.FindAll();
            }
        }
    }
}
