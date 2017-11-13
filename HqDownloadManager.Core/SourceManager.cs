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
        private readonly SiteHelper _siteHelper;


        public event ProcessingEventHandler ProcessingProgress;
        public event ProcessingErrorEventHandler ProcessingError;

        public SourceManager(DependencyInjection dependencyInjection) {
            _dependencyInjection = dependencyInjection;
            _siteHelper = _dependencyInjection
                .Resolve<SiteHelper>();
            _coveCacheHelper = dependencyInjection
               .Resolve<CoverCacheHelper>();
            _libraryContext = _dependencyInjection
                .Resolve<LibraryContext>();
        }

        public async Task<ModelBase> GetInfo(string url, bool isFinalized = false) {
            var model = default(ModelBase);
            if (_siteHelper.IsSupported(url)) {
                if (_siteHelper.IsHqPage(url)) {
                    var time = 72;
                    if (isFinalized) {
                        time = 999999999;
                    }
                    model = await CacheManagement(url, GetInfoFromSite<Hq>, time) as Hq;
                }
                if (_siteHelper.IsChapterReader(url)) {
                    model = await CacheManagement(url, GetInfoFromSite<Chapter>, 999999999) as Chapter;
                }
            }
            return model;
        }

        public async Task<LibraryPage> GetLibrary(string url) => await CacheManagement(url, GetLibraryFromSite, 168);

        public async Task<List<Hq>> GetUpdates(string url) => await CacheManagement(url, GetUpdatesFromSite, 1);

        private async Task<T> GetInfoFromSite<T>(string url) where T : ModelBase {
            var model = default(T);
            var source = _siteHelper.GetHqSourceFromUrl(url); ;
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

        private async Task<LibraryPage> GetLibraryFromSite(string url)  {
            var library = new LibraryPage();
            var source = _siteHelper.GetHqSourceFromUrl(url);
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

        private async Task<List<Hq>> GetUpdatesFromSite(string url) {
            var source = _siteHelper.GetHqSourceFromUrl(url);
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
