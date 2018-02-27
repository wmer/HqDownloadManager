using HqDownloadManager.Core.Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Managers {
    public class MangaLivreSourceManager : HqSourceManager<MangasProjectSource> {
        public MangaLivreSourceManager(CacheManager cacheManager, MangasProjectSource hqSource) : base(cacheManager, hqSource) {
            UpdatePage = "https://mangalivre.com/";
            LibraryPage = "https://mangalivre.com/lista-de-mangas/ordenar-por-nome/todos";
        }
    }
}
