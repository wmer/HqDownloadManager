using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Managers {
    public class MangaHostSourceManager : HqSourceManager<MangaHostSource> {

        public MangaHostSourceManager(CacheManager cacheManager, MangaHostSource mangaHost) : base(cacheManager, mangaHost) {
            UpdatePage = "https://mangashost.com/";
            LibraryPage = "https://mangahosts.com/mangas";
        }

        public IHqSourceManager Search(string hqTitle, out List<Hq> result) {
            result = _hqSource.Search(hqTitle);
            return this;
        }
    }
}
