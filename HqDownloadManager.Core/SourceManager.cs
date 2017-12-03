using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Core.Database;
using Newtonsoft.Json;
using Utils;

namespace HqDownloadManager.Core {
    public class SourceManager {
        private DependencyInjection _dependencyInjection;
        private readonly SiteHelper _siteHelper;
        private readonly LibraryContext _libraryContext;
        private readonly CoverCacheHelper _coveCacheHelper;

        private readonly Object _lockThis1 = new Object();
        private readonly Object _lockThis2 = new Object();
        private readonly Object _lockThis3 = new Object();
        private readonly Object _lockThis4 = new Object();
        private readonly Object _lockThis5 = new Object();

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


        public T GetInfo<T>(string url, bool isFinalized = false, int timeCache = 72) where T: ModelBase {
            lock (_lockThis1) {
                var model = default(T);
                if (_siteHelper.IsSupported(url)) {
                    if (typeof(T).IsAssignableFrom(typeof(Hq))) {
                        if (isFinalized) {
                            timeCache = 999999999;
                        }
                        if (_libraryContext.Hq.FindOne(url) is HqModel hqModel) {
                            var hq = hqModel.Hq.ToObject<Hq>();
                            if ((DateTime.Now - hqModel.TimeCache).Hours > timeCache) {
                                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Cache Vencido"));
                                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Atualizando..."));
                                var modelFromSite = GetInfoFromSite<Hq>(url);
                                foreach (var chap in modelFromSite.Chapters) {
                                    if (!hq.Chapters.Contains(chap)) {
                                        hq.Chapters.Add(chap);
                                    }
                                }
                                _libraryContext.Hq.Update(x => new { x.TimeCache, x.Hq }, DateTime.Now, hq.ToBytes())
                                                                         .Where(x => x.Link == url).Execute();
                            }
                            model = hq as T;
                        } else {
                            model = GetInfoFromSite<T>(url);
                            (model as Hq).IsFinalized = isFinalized;
                            _libraryContext.Hq.Save(new HqModel {
                                Link = model.Link, TimeCache = DateTime.Now, Hq = model.ToBytes()
                            });
                        }
                    }
                    if (typeof(T).IsAssignableFrom(typeof(Chapter))) {
                        model = GetInfoFromSite<T>(url);
                    }
                }
                return model;
            }
        }

        public LibraryPage GetLibrary(string url) => CacheManagement(url, GetLibraryFromSite, 168);

        public List<Update> GetUpdates(string url) => CacheManagement(url, GetUpdatesFromSite, 1);

        private T CacheManagement<T>(string url, Func<String, T> method, int timeCache) {
            lock (_lockThis5) {
                var model = default(T);
                if (_siteHelper.IsSupported(url)) {
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
                }
                return model;
            }
        }

        private T GetInfoFromSite<T>(string url) where T : ModelBase {
            lock (_lockThis3) {
                var model = default(T);
                var source = _siteHelper.GetHqSourceFromUrl(url);
                source.ProcessingProgress += Source_ProcessingProgress;
                source.ProcessingError += SourceOnProcessingError;
                if (typeof(T).IsAssignableFrom(typeof(Hq))) {
                    model = source.GetHqInfo(url) as T;
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Criando cache de Capa..."));
                    _coveCacheHelper.CreateCache(model as Hq);
                }
                if (typeof(T).IsAssignableFrom(typeof(Chapter))) {
                    model = source.GetChapterInfo(url) as T;
                }
                return model;
            }
        }

        private LibraryPage GetLibraryFromSite(string url) {
            lock (_lockThis4) {
                var library = new LibraryPage();
                if (_siteHelper.IsSupported(url)) {
                    var source = _siteHelper.GetHqSourceFromUrl(url);
                    source.ProcessingProgress += Source_ProcessingProgress;
                    source.ProcessingError += SourceOnProcessingError;
                    library = source.GetLibrary(url);
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Criando cache de Capa..."));

                    foreach (var hq in library.Hqs) {
                        _coveCacheHelper.CreateCache(hq);
                    }
                }

                return library;
            }
        }

        private List<Update> GetUpdatesFromSite(string url) {
            lock (_lockThis5) {
                var source = _siteHelper.GetHqSourceFromUrl(url);
                source.ProcessingProgress += Source_ProcessingProgress;
                source.ProcessingError += SourceOnProcessingError;
                var updates = source.GetUpdates(url);
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Criando cache de Capa..."));
                foreach (var hq in updates) {
                    if (!string.IsNullOrEmpty(hq.Hq.Link)) {
                        _coveCacheHelper.CreateCache(hq.Hq);
                    }
                }
                return updates;
            }
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
