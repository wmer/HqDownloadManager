using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Managers {
    public class MangasProjectSourceManager : HqSourceManager<MangasProjectSource> {
        public MangasProjectSourceManager(CacheManager cacheManager, MangasProjectSource hqSource) : base(cacheManager, hqSource) {
            UpdatePage = "https://mangas.zlx.com.br";
            LibraryPage = "https://mangas.zlx.com.br/lista-de-mangas/ordenar-por-nome/todos";
        }
    }
}
