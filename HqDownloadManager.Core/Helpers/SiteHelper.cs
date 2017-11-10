using DependencyInjectionResolver;
using HqDownloadManager.Core.Sources;
using Reflection.Optimization.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Helpers {
    internal class SiteHelper {
        private string _webDriverPath;
        private List<String> SupportedSites;
        private Object lockThis = new Object();
        private Object lockThis2 = new Object();
        private Object lockThis3 = new Object();
        private Object lockThis4 = new Object();

        public SiteHelper(string webDriverPath) {
            _webDriverPath = webDriverPath;
            SupportedSites = new List<String> { "ymangas", "yesmangas", "mangashost", "mangahost", "unionmangas", "mangas", "mangastream",
                "kissmanga", "readms", "mangafox" };
        }

        public IHqSource GetHqSourceFromUrl(String link) {
            lock (lockThis) {
                Uri url = new Uri(link);
                var host = url.Host.Split('.')[0];
                if (SupportedSites.Contains(host)) {
                    if ((host == "mangastream" || host == "mangas")) {
                        host = "mangasproject";
                    }
                    if (host == "readms") {
                        host = "mangastream";
                    }
                    if (host == "mangashost") {
                        host = "mangahost";
                    }
                    if (host == "ymangas") {
                        host = "yesmangas";
                    }
                    var type = AssemblyHelper.GetType(typeof(IHqSource).GetTypeInfo().Assembly, $"{host}source");
                    return new DependencyInjection()
                                                .BindingTypes(typeof(IHqSource), type)
                                                .DefineDependency<BrowserHelper>(0, _webDriverPath)
                                                .Resolve<IHqSource>();
                }
                throw new Exception("Não é possivel fazer download deste site!");
            }
        }

        public bool IsHqPage(String link) {
            lock (lockThis2) {
                Uri url = new Uri(link);
                var host = url.Host.Split('.')[0];
                if (SupportedSites.Contains(host)) {
                    if ((host == "mangas" || host == "mangastream")) {
                        if (url.Segments.Count() == 4 && (url.Segments[1].ToLower() == "manga/" || url.Segments[1].ToLower() == "titulos/")) {
                            return true;
                        }
                    } else {
                        if (url.Segments.Count() == 3 && (url.Segments[1].ToLower() == "manga/" || url.Segments[1].ToLower() == "titulos/")) {
                            return true;
                        }
                    }

                    return false;
                }
                throw new Exception("Este site ainda não é suportado!");
            }
        }

        public bool IsChapterReader(String link) {
            lock (lockThis3) {
                Uri url = new Uri(link);
                var host = url.Host.Split('.')[0];
                if (SupportedSites.Contains(host)) {
                    var count = url.Segments.Count();
                    var page = url.Segments[1];

                    if ((host == "mangas" || host == "mangastream")) {
                        if (url.Segments.Count() > 4 && (url.Segments[1].ToLower() == "manga/" || url.Segments[1].ToLower() == "titulos/" || url.Segments[1].ToLower() == "leitor/" || url.Segments[1].ToLower() == "r/")) {
                            return true;
                        }
                    } else {
                        if (url.Segments.Count() > 3 && (url.Segments[1].ToLower() == "manga/" || url.Segments[1].ToLower() == "titulos/" || url.Segments[1].ToLower() == "leitor/" || url.Segments[1].ToLower() == "r/")) {
                            return true;
                        }
                    }

                    return false;
                }
                throw new Exception("Este site ainda não é suportado!");
            }
        }

        public bool IsSupported(string link) {
            lock (lockThis4) {
                Uri url = new Uri(link);
                var host = url.Host.Split('.')[0];
                return SupportedSites.Contains(host);
            }
        }
    }
}
