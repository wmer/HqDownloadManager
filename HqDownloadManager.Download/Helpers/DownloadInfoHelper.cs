using HqDownloadManager.Core.Models;
using HqDownloadManager.Download.Databases;
using HqDownloadManager.Download.Models;
using HqDownloadManager.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Download.Helpers
{
    internal class DownloadInfoHelper {
        private readonly DownloadContext _downloadContext;
        private readonly object _lock = new object();
        private readonly object _lock2 = new object();


        public DownloadInfoHelper(DownloadContext downloadContext) {
            _downloadContext = downloadContext;
        }

        public async Task SaveHqDownloadInfo(Hq hq, string path, DateTime time) {
           await Task.Run(()=> {
                lock (_lock) {
                    var dInfo = new HqDownloadInfo {
                        Link = hq.Link,
                        SavedIn = path, Time = time,
                        HqDownloaded = hq.ToBytes()
                    };
                    if (_downloadContext.HqDownloadInfo.Find().Where(x => x.SavedIn == path).Execute().FirstOrDefault() != null) {
                        _downloadContext.HqDownloadInfo.Update(dInfo);
                    } else {
                        _downloadContext.HqDownloadInfo.Save(dInfo);
                    }
                }
            });          
        }

        public async Task<List<HqDownloadInfo>> GetHqsDownloadInfo() {
            return await Task.Factory.StartNew<List<HqDownloadInfo>>(()=> {
                lock (_lock2) {
                    return _downloadContext.HqDownloadInfo.FindAll();
                }
            });
        }

        public async Task DeleteDownloadInfo(string link, bool deleteFiles = false) {
            await Task.Run(()=> {
                if (_downloadContext.HqDownloadInfo.FindOne(link) is HqDownloadInfo hqDownloadInfo) {
                    _downloadContext.HqDownloadInfo.Delete(hqDownloadInfo);
                    if (deleteFiles) {
                        Directory.Delete(hqDownloadInfo.SavedIn, true);
                    }
                }
            });
        }
    }
}
