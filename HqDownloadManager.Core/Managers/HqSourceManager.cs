using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Sources;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Managers {
    public abstract class HqSourceManager<T> : IHqSourceManager where T : HqSource {
        protected readonly CacheManager _cacheManager;
        protected readonly T _hqSource;

        protected string UpdatePage { get; set; }
        protected string LibraryPage { get; set; }
        protected string FinalizedPage { get; set; }
        protected string NextPage { get; set; }
        protected Dictionary<string, string> Lethers { get; set; }

        private readonly object _lockThis = new object();
        private readonly object _lockThis2 = new object();
        private readonly object _lockThis3 = new object();
        private readonly object _lockThis4 = new object();

        public HqSourceManager(CacheManager cacheManager, T hqSource) {
            _cacheManager = cacheManager;
            _hqSource = hqSource;
        }

        public virtual IHqSourceManager GetInfo<U>(string url, out U model, double timeCache, bool isFinalized, bool withoutCache) where U : ModelBase {
            lock (_lockThis4) {
                model = _cacheManager.ModelCache<U>(url, GetInfoFromSite<U>, timeCache, isFinalized, withoutCache);
                return this;
            }
        }

        public virtual IHqSourceManager GetUpdates(out List<Update> updates, double timeCache) {
            lock (_lockThis3) {
                updates = _cacheManager.UpdatesCache(UpdatePage, _hqSource.GetUpdates, timeCache);
                return this;
            }
        }

        public virtual IHqSourceManager GetLibrary(out LibraryPage library, double timeCache) =>
                                                            GetLibrary(LibraryPage, out library, timeCache);
        public virtual IHqSourceManager GetFinalizedPage(out LibraryPage library, double timeCache) =>
                                                            GetLibrary(FinalizedPage, out library, timeCache);
        public virtual IHqSourceManager GetLetherPage(string lether, out LibraryPage library, double timeCache) {
            library = new LibraryPage();
            if (Lethers.ContainsKey(lether)) {
                GetLibrary(Lethers[lether], out library, timeCache);
            }
            return this;
        }

        public virtual IHqSourceManager NextLibraryPage(out LibraryPage library, double timeCache) =>
                                                            GetLibrary(NextPage, out library, timeCache);

        protected virtual IHqSourceManager GetLibrary(string url, out LibraryPage library, double timeCache) {
            lock (_lockThis) {
                library = new LibraryPage();
                library = _cacheManager.CacheManagement(url, _hqSource.GetLibrary, timeCache);
                NextPage = library.NextPage;
                FinalizedPage = library.FinalizedPage;
                if (library.Letras != null && library.Letras.Count > 0) {
                    Lethers = library.Letras;
                }
                return this;
            }
        }

        private U GetInfoFromSite<U>(string url) where U : ModelBase {
            lock (_lockThis2) {
                var model = default(U);
                if (typeof(U).IsAssignableFrom(typeof(Hq))) {
                    model = _hqSource.GetHqInfo(url) as U;
                }
                if (typeof(U).IsAssignableFrom(typeof(Chapter))) {
                    model = _hqSource.GetChapterInfo(url) as U;
                }

                return model;
            }
        }
    }
}
