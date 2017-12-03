using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using DependencyInjectionResolver;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Database;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;

namespace HqDownloadManager.Core.Sources {
    internal class HipercoolSource : HqSource {
        private string BaseAdress { get; set; }
        private Uri Site { get; set; }

        public HipercoolSource(LibraryContext libraryContext, HtmlSourceHelper htmlHelper, BrowserHelper browserHelper) : base(libraryContext, htmlHelper, browserHelper) {
        }

        public override List<Update> GetUpdates(String updatePage) {
            lock (Lock7) {
                try {
                    Uri site = new Uri(updatePage);
                    BaseAdress = $"{site.Scheme}://{site.Host}";
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando Lançamentos..."));
                    var source = HtmlHelper.GetSourceCodeFromUrl(updatePage);
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                    var updates = new List<Update>();
                    var chapters = new List<Chapter>();
                    if (source == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
                    var updatesEl = source.QuerySelectorAll("div.cap-card");
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"{updatesEl.Count()} mangas encontrados!"));
                    foreach (var update in updatesEl) {
                        var title = update.QuerySelector(".title-area")?.TextContent;
                        if (!string.IsNullOrEmpty(title)) {
                            title = title.Replace("\n", "").Trim();
                        }
                        var img = update.QuerySelector("a.container img")?.GetAttribute("src");
                        var link = update.QuerySelector("a.container")?.GetAttribute("href");
                        if (!string.IsNullOrEmpty(link)) {
                            var up = new Update {
                                Hq = new Hq { Link = $"{BaseAdress}{link}", Title = title, CoverSource = $"{Site.Scheme}:{img}" },
                                Chapters = chapters
                            };
                            updates.Add(up);
                            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, up.Hq, $"{title} Adicionado"));
                        }
                    }
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                    return updates; 
                } catch (Exception e) {
                    OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, updatePage, e));
                    return null;
                }
            }
        }

        public override LibraryPage GetLibrary(String linkPage) {
            lock (Lock6) {
                try {
                    Site = new Uri(linkPage);
                    BaseAdress = $"{Site.Scheme}://{Site.Host}";
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, "Processando os Mangas..."));
                    var driver = BrowserHelper.GetDriver(linkPage);
                    Task.Delay(5000).Wait();
                    var pageSource = driver.PageSource;
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                    IDocument source = HtmlHelper.GetSourceCodeFromHtml(pageSource);
                    driver.Quit();
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                    var hqs = new List<Hq>();
                    if (source == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
                    var hqsEl = source.QuerySelectorAll(".mdl-data-table tbody tr");
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"{hqsEl.Count()} mangas encontrados!"));
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Preparando para retornar mangas!"));
                    foreach (var hq in hqsEl) {
                        var link = hq.QuerySelector("td a")?.GetAttribute("href");
                        if (!string.IsNullOrEmpty(link)) {
                            var manga = new SourceManager(new DependencyInjection()).GetInfo<Hq>($"{BaseAdress}{link}");
                        }
                    }
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                    return new LibraryPage {
                        Hqs = hqs
                    };
                } catch (Exception e) {
                    OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, linkPage, e));
                    return null;
                }
            }
        }

        public override Hq GetHqInfo(string link) {
            lock (Lock1) {
                try {
                    Site = new Uri(link);
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                    var source = HtmlHelper.GetSourceCodeFromUrl(link);
                    var hqInfo = new Hq();
                    if (source == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
                    var title = source.QuerySelector(".manga-card .infos-wrapper .title")?.TextContent;
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {title}"));
                    var coverEl = source.QuerySelector(".manga-card .cover-wrapper img");
                    var synopsis = source.QuerySelector(".manga-card .infos-wrapper .sinopse")?.TextContent;
                    hqInfo.Title = title;
                    var img = coverEl?.GetAttribute("src");
                    if (!string.IsNullOrEmpty(img)) {
                        hqInfo.CoverSource = $"{Site.Scheme}:{img}";
                    }
                    hqInfo.Synopsis = synopsis?.Replace("\n", "").Trim(); ;
                    hqInfo.Link = link;
                    var hqInfos = source.QuerySelectorAll(".manga-card .infos-wrapper .infos .info");
                    foreach (var info in hqInfos) {
                        var txt = info.TextContent;
                        if (info.TextContent.Contains("Autor")) {
                            hqInfo.Author = info.TextContent.Replace("Autor:", "").Trim();
                        }
                    }
                    var chapters = GetListChapters(source).Reverse<Chapter>().ToList();
                    hqInfo.Chapters = chapters;

                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto!"));
                    return hqInfo;
                } catch (Exception e) {
                    OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, link, e));
                    return null;
                }
            }
        }

        public override Chapter GetChapterInfo(string link) {
            lock (Lock2) {
                try {
                    Site = new Uri(link);
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                    IDocument leitor = HtmlHelper.GetSourceCodeFromUrl(link);
                    var chapter = new Chapter();
                    if (leitor == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
                    var chapterTitle = leitor.QuerySelector(".main-container .title")?.TextContent.Trim();
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {chapterTitle}"));
                    chapter.Title = chapterTitle;
                    chapter.Link = link;
                    var pages = GetPageList(leitor);
                    if (pages.Count <= 2) {
                        UsingIe = true;
                        var driver = BrowserHelper.GetDriver(link);
                        var pageSource = driver.PageSource;
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                        leitor = HtmlHelper.GetSourceCodeFromHtml(pageSource);
                        pages = GetPageList(leitor);
                        driver.Quit();
                        UsingIe = false;
                    }
                    chapter.Pages = pages;
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                    return chapter;
                } catch (Exception e) {
                    OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, link, e));
                    return null;
                }
            }
        }

        public List<Chapter> GetListChapters(IDocument hqSource) {
            lock (Lock3) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando capitulos"));
                var chapterList = new List<Chapter>();
                var chaptersContent = hqSource.QuerySelector(".manga-wrapper .cards-wrapper");
                if (chaptersContent == null) {
                    var chapterLink = hqSource.QuerySelector(".infos-wrapper .infos-buttons .button")?.GetAttribute("href"); ;
                    var chapterTitle = $"01";
                    var chap = new Chapter { Link = $"{BaseAdress}{chapterLink}", Title = chapterTitle };
                    chapterList.Add(chap);
                } else {
                    var chapters = chaptersContent.QuerySelectorAll(".cap-card");
                    if(chapters != null && chapters.Length > 0) {
                        foreach (var chapter in chapters) {
                            var chapterLink = chapter.QuerySelector(".buttons-wrapper a")?.GetAttribute("href");
                            var chapterTitle = chapter.QuerySelector(".content .title-area .title")?.TextContent;
                            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Adicionando {chapterTitle}"));
                            var chap = new Chapter { Link = $"{BaseAdress}{chapterLink}", Title = chapterTitle };
                            chapterList.Add(chap);
                        }
                    }
                }
                return chapterList;
            }
        }

        public List<Models.Page> GetPageList(IDocument chapterSoure) {
            lock (Lock4) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando páginas"));
                var pageList = new List<Models.Page>();
                var mangaPages = chapterSoure.QuerySelectorAll(".pages-wrapper img");
                if (mangaPages == null) return pageList;
                var num = 1;
                foreach (var img in mangaPages) {
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Adicionando página {num}"));
                    var page = new Page { Number = num, Source = $"{Site.Scheme}:{img.GetAttribute("src")}" };
                    pageList.Add(page);
                    num++;
                }
                return pageList;
            }
        }
    }
}
