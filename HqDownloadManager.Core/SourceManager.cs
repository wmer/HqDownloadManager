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
using Cache = HqDownloadManager.Core.Models.Cache;

namespace HqDownloadManager.Core {
    public class SourceManager {
        private DependencyInjection _dependencyInjection;
        private readonly SiteHelper _siteHelper;
        private readonly LibraryContext _libraryContext;
        private readonly CoverCacheHelper _coveCacheHelper;

        private readonly Object _lockThis = new Object();
        private readonly Object _lockThus2 = new Object();
        private readonly Object _lockThis3 = new Object();
        private readonly Object _lockThis4 = new Object();
        private readonly Object _lockThis5 = new Object();
        private readonly Object _lockThis6 = new Object();
        private readonly Object _lockThis7 = new Object();

        public event ProcessingEventHandler ProcessingProgress;

        public SourceManager(DependencyInjection dependencyInjection) {
            _dependencyInjection = dependencyInjection;
            _siteHelper = _dependencyInjection.Resolve<SiteHelper>();
            _coveCacheHelper = dependencyInjection.Resolve<CoverCacheHelper>();
            _libraryContext = _dependencyInjection
                                    .DefineConstructorSignature<LibraryContext>(Type.EmptyTypes)
                                    .Resolve<LibraryContext>();
        }

        public ModelBase GetInfo(string url, bool isFinalized = false) {
            lock (_lockThis7) {
                var model = new ModelBase();
                if (_siteHelper.IsSupported(url)) {
                    if (_siteHelper.IsHqPage(url)) {
                        var time = 72;
                        if (isFinalized) {
                            time = 999999999;
                        }
                        model = CacheManagement(url, GetInfoFromSite<Hq>, time);
                    }
                    if (_siteHelper.IsChapterReader(url)) {
                        model = CacheManagement(url, GetInfoFromSite<Chapter>, 999999999);
                    }
                }
                return model;
            }
        }

        public LibraryPage GetLibrary(string url) => CacheManagement(url, GetLibraryFromSite, 168);

        public List<Hq> GetUpdates(string url) => CacheManagement(url, GetUpdatesFromSite, 1);

        private T CacheManagement<T>(string url, Func<String, T> method, int timeCache) {
            lock (_lockThis6) {
                var model = default(T);
                if (_siteHelper.IsSupported(url)) {
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando em DB"));
                    if (_libraryContext.Cache.FindOne(url) is Cache cache) {
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Encontrado!"));
                        if ((DateTime.Now - cache.Date).Hours < timeCache) {
                            model = JsonConvert.DeserializeObject<T>(Encoding.ASCII.GetString(cache.ModelsCache));
                        } else {
                            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Cache Vencido"));
                            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Atualizando..."));
                            model = method.Invoke(url);
                            _libraryContext.Cache.Update(x => new { x.ModelsCache, x.Date },
                                Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(model)), DateTime.Now)
                                .Where(x => x.Link == url).Execute();
                        }
                    } else {
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Não encontrado!"));
                        model = method.Invoke(url);
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Criando Cache"));
                        var updt = new Cache {
                            Link = url, Date = DateTime.Now, ModelsCache =
                            Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(model))
                        };
                        _libraryContext.Cache.Save(updt);
                    }
                }
                return model;
            }
        }

        private T GetInfoFromSite<T>(string url) where T : ModelBase {
            lock (_lockThis) {
                var model = default(T);
                var source = _siteHelper.GetHqSourceFromUrl(url);
                source.ProcessingProgress += Source_ProcessingProgress;
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
            lock (_lockThus2) {
                var library = new LibraryPage();
                if (_siteHelper.IsSupported(url)) {
                    var source = _siteHelper.GetHqSourceFromUrl(url);
                    source.ProcessingProgress += Source_ProcessingProgress;
                    library = source.GetLibrary(url);
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Criando cache de Capa..."));
                    foreach (var hq in library.Hqs) {
                        _coveCacheHelper.CreateCache(hq);
                    }
                }

                return library;
            }
        }

        private List<Hq> GetUpdatesFromSite(string url) {
            lock (_lockThis5) {
                var source = _siteHelper.GetHqSourceFromUrl(url);
                source.ProcessingProgress += Source_ProcessingProgress;
                var updates = source.GetUpdates(url);
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Criando cache de Capa..."));
                foreach (var hq in updates) {
                    if (!string.IsNullOrEmpty(hq.Link)) {
                        _coveCacheHelper.CreateCache(hq);
                    }
                }
                return updates;
            }
        }

        private void Source_ProcessingProgress(object sender, ProcessingEventArgs ev) {
            lock (_lockThis3) {
                OnProcessingProgress(ev);
            }
        }

        protected void OnProcessingProgress(ProcessingEventArgs e) {
            lock (_lockThis4) {
                ProcessingProgress?.Invoke(this, e);
            }
        }
    }
}
