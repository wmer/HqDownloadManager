using DependencyInjectionResolver;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Database;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Sources;
using HqDownloadManager.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core {
    public class SourceManager {
        private DependencyInjection _dependencyInjection;
        private readonly LibraryContext _libraryContext;
        private readonly CoverCacheHelper _coveCacheHelper;
        

        public event ProcessingEventHandler ProcessingProgress;
        public event ProcessingErrorEventHandler ProcessingError;

        public SourceManager(DependencyInjection dependencyInjection) {
            _dependencyInjection = dependencyInjection;
            _coveCacheHelper = dependencyInjection
               .Resolve<CoverCacheHelper>();
            _libraryContext = _dependencyInjection
                .Resolve<LibraryContext>();
        }

        public async Task<T> GetInfo<T, U>(string url, bool isFinalized = false) where T : ModelBase where U : HqSource {
            var model = default(T);
            if (typeof(T).IsAssignableFrom(typeof(Hq))) {
                var time = 72;
                if (isFinalized) {
                    time = 999999999;
                }
                model = await CacheManagement(url, GetInfoFromSite<Hq, U>, time) as T;
            }
            if (typeof(T).IsAssignableFrom(typeof(Chapter))) {
                model = await CacheManagement(url, GetInfoFromSite<Chapter, U>, 999999999) as T;
            }
            return model;
        }

        public async Task<LibraryPage> GetLibrary<U>(string url) where U : HqSource => await CacheManagement(url, GetLibraryFromSite<U>, 168);

        public async Task<List<Hq>> GetUpdates<U>(string url) where U : HqSource => await CacheManagement(url, GetUpdatesFromSite<U>, 1);

        private async Task<T> GetInfoFromSite<T, U>(string url) where T : ModelBase where U : HqSource {
            var model = default(T);
            var source = _dependencyInjection.Resolve<U>();
            source.ProcessingProgress += Source_ProcessingProgress;
            source.ProcessingError += SourceOnProcessingError;
            if (typeof(T).IsAssignableFrom(typeof(Hq))) {
                model = await source.GetHqInfo(url) as T;
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Criando cache de Capa..."));
                _coveCacheHelper.CreateCache(model as Hq);
            }
            if (typeof(T).IsAssignableFrom(typeof(Chapter))) {
                model = await source.GetChapterInfo(url) as T;
            }
            return model;
        }

        private async Task<LibraryPage> GetLibraryFromSite<U>(string url) where U : HqSource {
            var library = new LibraryPage();
            var source = _dependencyInjection.Resolve<U>();
            source.ProcessingProgress += Source_ProcessingProgress;
            source.ProcessingError += SourceOnProcessingError;
            library = await source.GetLibrary(url);
            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Criando cache de Capa..."));

            Task.Run(()=> {
                foreach (var hq in library.Hqs) {
                    _coveCacheHelper.CreateCache(hq);
                }
            });

            return library;
        }

        private async Task<List<Hq>> GetUpdatesFromSite<U>(string url) where U : HqSource {
            var source = _dependencyInjection.Resolve<U>();
            source.ProcessingProgress += Source_ProcessingProgress;
            source.ProcessingError += SourceOnProcessingError;
            var updates = await source.GetUpdates(url);
            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Criando cache de Capa..."));
            foreach (var hq in updates) {
                if (!string.IsNullOrEmpty(hq.Link)) {
                    _coveCacheHelper.CreateCache(hq);
                }
            }
            return updates;
        }

        private T CacheManagement<T>(string url, Func<String, T> method, int timeCache) {
            var model = default(T);
            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando em DB"));
            if (_libraryContext.Cache.FindOne(url) is Cache cache) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Encontrado!"));
                if ((DateTime.Now - cache.Date).Hours < timeCache) {
                    model = cache.ModelsCache.ToObject<T>();
                } else {
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Cache Vencido"));
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Atualizando..."));
                    model = method.Invoke(url);
                    _libraryContext.Cache.Update(x => new { x.ModelsCache, x.Date }, model.ToBytes(), DateTime.Now)
                                                             .Where(x => x.Link == url).Execute();
                }
            } else {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Não encontrado!"));
                model = method.Invoke(url);
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Criando Cache"));
                var updt = new Cache {
                    Link = url, Date = DateTime.Now, ModelsCache = model.ToBytes()
                };
                _libraryContext.Cache.Save(updt);
            }
            return model;
        }

        private void SourceOnProcessingError(object sender, ProcessingErrorEventArgs ev) =>
                OnProcessingProgressError(ev);

        private void Source_ProcessingProgress(object sender, ProcessingEventArgs ev) =>
                OnProcessingProgress(ev);

        protected void OnProcessingProgress(ProcessingEventArgs e) =>
                ProcessingProgress?.Invoke(this, e);

        protected void OnProcessingProgressError(ProcessingErrorEventArgs e) =>
                ProcessingError?.Invoke(this, e);
    }
}
