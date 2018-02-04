using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Managers {
    public class HqUltimateSourceManager : HqSourceManager<HqUltimateSource> {
        public HqUltimateSourceManager(CacheManager cacheManager, HqUltimateSource hqSource) : base(cacheManager, hqSource) {
            UpdatePage = "https://hqultimate.com/";
            LibraryPage = "http://hqultimate.com/hqs";
        }
    }
}
