using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Managers {
    public class UnionMangasSourceManager : HqSourceManager<UnionMangasSource> {
        public UnionMangasSourceManager(CacheManager cacheManager, UnionMangasSource hqSource) : base(cacheManager, hqSource) {
            UpdatePage = "http://unionmangas.net/";
            LibraryPage = "http://unionmangas.net/mangas";
        }
    }
}
