using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;

namespace HqDownloadManager.Core.Sources {
    public class MangaHostSource : HqSource {
        public MangaHostSource(HtmlSourceHelper htmlHelper, BrowserHelper browserHelper) : base(htmlHelper, browserHelper) {
        }

        public override async Task<Chapter> GetChapterInfo(string link) {
            try {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                var driver = await BrowserHelper.GetPhantomJSMobile(link);
                var pageSource = driver.PageSource;
                IDocument leitor = HtmlHelper.GetSourceCodeFromHtml(pageSource);
                driver.Quit();
                var chapter = new Chapter();
                if (leitor == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
                var chapterTitle = leitor.QuerySelector("div.breadcrumb ul.container  li.active span")?.TextContent;
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {chapterTitle}"));
                chapter.Title = chapterTitle;
                chapter.Link = link;
                var pages = GetPageList(leitor);
                chapter.Pages = pages;
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                return chapter;
            } catch (Exception e) {
                OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, link, e));
                return null;
            }
        }

        public override async Task<Hq> GetHqInfo(string link) {
            try {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                var source = await HtmlHelper.GetSourceCodeFromUrl(link);
                var hqInfo = new Hq();
                if (source == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
                var title = source.QuerySelector(".title-widget .entry-title")?.TextContent;
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {title}"));
                var img = source.QuerySelector("div#page img");
                var synopsis = source.QuerySelector(".pull-left article")?.TextContent;
                hqInfo.Title = title;
                hqInfo.CoverSource = img?.GetAttribute("src");
                hqInfo.Synopsis = synopsis?.Replace("\n", "").Trim(); ;
                hqInfo.Link = link; var hqInfos = source.QuerySelectorAll("ul.descricao  li");
                foreach (var info in hqInfos) {
                    if (info.TextContent.Contains("Autor")) {
                        hqInfo.Author = info.TextContent.Replace("Autor:", "").Trim();
                    }
                }
                hqInfo.Chapters = GetListChapters(source).Reverse<Chapter>().ToList();

                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto!"));
                return hqInfo;
            } catch (Exception e) {
                OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, link, e));
                return null;
            }
        }

        public override async Task<LibraryPage> GetLibrary(string linkPage) {
            try {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, "Processando os Mangas..."));
                var source = await HtmlHelper.GetSourceCodeFromUrl(linkPage);

                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));

                var hqs = new List<Hq>();
                if (source == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
                var hqsEl = source.QuerySelectorAll(".container #page .list .thumbnail");
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"{hqsEl.Count()} mangas encontrados!"));
                var nextPage = source.QuerySelector("div.wp-pagenavi a.nextpostslink")?.GetAttribute("href");
                var finalizadosEl = source.QuerySelectorAll(".container #page ul.nav-tabs li a");
                var finalizedPage = "";
                foreach (var el in finalizadosEl) {
                    if (el.TextContent.Contains("Completos")) {
                        finalizedPage = el.GetAttribute("href");
                    }
                }
                var lethers = source.QuerySelectorAll(".paginador ul.letras li a");
                var letherLink = new Dictionary<string, string>();
                foreach (var lether in lethers) {
                    letherLink[lether.TextContent] = lether.GetAttribute("href");
                }
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Preparando para retornar mangas!"));
                foreach (var hq in hqsEl) {
                    var title = hq.QuerySelector("h3")?.TextContent;
                    var img = hq.QuerySelector("a img")?.GetAttribute("src");
                    var link = hq.QuerySelector("h3 a")?.GetAttribute("href");
                    if (!string.IsNullOrEmpty(link)) {
                        var manga = new Hq { Link = link, Title = title, CoverSource = img };
                        hqs.Add(manga);
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, manga, $"{title} Adicionado"));
                    }
                }
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                return new LibraryPage {
                    Hqs = hqs, NextPage = nextPage, FinalizedPage = finalizedPage, Letras = letherLink
                };
            } catch (Exception e) {
                OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, linkPage, e));
                return null;
            }
        }

        public override async Task<List<Hq>> GetUpdates(string updatePage) {
            try {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando Lançamentos..."));
                var source = await HtmlHelper.GetSourceCodeFromUrl(updatePage);
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                var hqs = new List<Hq>();
                if (source == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
                var hqsEl = source.QuerySelectorAll(".table-lancamentos tr");
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"{hqsEl.Count()} mangas encontrados!"));
                foreach (var hq in hqsEl) {
                    var title = hq.QuerySelector("td h3.entry-title")?.TextContent;
                    var img = hq.QuerySelector("td a img")?.GetAttribute("src");
                    var link = hq.QuerySelector("td h3.entry-title a")?.GetAttribute("href");
                    if (!string.IsNullOrEmpty(link)) {
                        var update = new Hq { Link = link, Title = title, CoverSource = img };
                        hqs.Add(update);
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, update, $"{title} Adicionado"));
                    }
                }
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                return hqs;
            } catch(Exception e) {
                OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, updatePage, e));
                return null;
            }
        }

        private List<Chapter> GetListChapters(IDocument hqSource) {
            lock (Lock1) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando capitulos"));
                var chapterList = new List<Chapter>();
                var chapters = hqSource.QuerySelectorAll("table a.capitulo");
                if (chapters != null && chapters.Length > 0) {
                    foreach (var chapter in chapters) {
                        var chapterLink = chapter.GetAttribute("href");
                        var chapterTitle = $"{chapter.TextContent}";
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Adicionando {chapterTitle}"));
                        var chap = new Chapter { Link = chapterLink, Title = chapterTitle };
                        chapterList.Add(chap);
                    }
                } else {
                    chapters = hqSource.QuerySelectorAll(".list_chapters a");
                    if (chapters == null) return chapterList;
                    foreach (var chapterEl in chapters) {
                        var chapter = HtmlHelper.GetSourceCodeFromHtml(chapterEl.GetAttribute("data-content"));
                        var linkEl = chapter.QuerySelector("a");
                        var chapterLink = linkEl?.GetAttribute("href");
                        var chapterTitle = $"Capitulo - {chapterEl.TextContent}";
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Adicionando {chapterTitle}"));
                        var chap = new Chapter { Link = chapterLink, Title = chapterTitle };
                        chapterList.Add(chap);
                    }
                }
                return chapterList;
            }
        }

        private List<Models.Page> GetPageList(IDocument chapterSoure) {
            lock (Lock2) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando páginas"));
                var pageList = new List<Models.Page>();
                var divPages = chapterSoure.QuerySelector("div.image-content");
                var mangaPages = divPages.QuerySelectorAll("a img");
                if (mangaPages == null) return pageList;
                var num = 1;
                foreach (var img in mangaPages) {
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Adicionando página {num}"));
                    var page = new Page { Number = num, Source = img.GetAttribute("src") };
                    pageList.Add(page);
                    num++;
                }
                return pageList;
            }
        }
    }
}
