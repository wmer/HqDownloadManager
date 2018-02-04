using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Managers {
    public class YesMangasSourceManager : HqSourceManager<YesMangasSource> {
        public YesMangasSourceManager(CacheManager cacheManager, YesMangasSource hqSource) : base(cacheManager, hqSource) {
            UpdatePage = "https://ymangas.com/";
            LibraryPage = "https://ymangas.com/mangas";
        }
    }
}
