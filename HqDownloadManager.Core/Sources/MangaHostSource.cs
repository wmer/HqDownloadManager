using HqDownloadManager.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.Models;
using AngleSharp.Dom;
using HqDownloadManager.Core.CustomEventArgs;
using MSHTML;
using HqDownloadManager.Core.Database;

namespace HqDownloadManager.Core.Sources {
    internal class MangaHostSource : HqSource {
        public HtmlSourceHelper htmlHelper;
        public BrowserHelper browserHelper;
        private bool usingIe;
        private object lock1 = new object();
        private object lock2 = new object();
        private object lock3 = new object();
        private object lock4 = new object();
        private object lock5 = new object();
        private object lock6 = new object();
        private object lock7 = new object();

        public MangaHostSource(LibraryContext libraryContext, HtmlSourceHelper htmlHelper, BrowserHelper browserHelper) : base(libraryContext) {
            this.htmlHelper = htmlHelper;
            this.browserHelper = browserHelper;
        }

        public override List<Hq> GetUpdates(String updatePage) {
            lock (lock7) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando Lançamentos..."));
                var source = htmlHelper.GetSourceCodeFromUrl(updatePage);
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                var hqs = new List<Hq>();
                if (source != null) {
                    var hqsEl = source.QuerySelectorAll(".table-lancamentos tr");
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"{hqsEl.Count()} mangas encontrados!"));
                    foreach (var hq in hqsEl) {
                        var title = hq.QuerySelector("td h3.entry-title")?.TextContent;
                        var img = hq.QuerySelector("td a img")?.GetAttribute("src");
                        var link = hq.QuerySelector("td h3.entry-title a")?.GetAttribute("href");
                        var update = new Hq { Link = link, Title = title, CoverSource = img };
                        hqs.Add(update);
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, update, $"{title} Adicionado"));
                    }
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                    return hqs;
                }
                throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
            }
        }

        public override LibraryPage GetLibrary(String linkPage) {
            lock (lock6) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, "Processando os Mangas..."));
                var source = htmlHelper.GetSourceCodeFromUrl(linkPage);

                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                var hqs = new List<Hq>();
                if (source != null) {
                    var hqsEl = source.QuerySelectorAll(".container #page .list .thumbnail");
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"{hqsEl.Count()} mangas encontrados!"));
                    var nextPage = source.QuerySelector("div.wp-pagenavi a.nextpostslink")?.GetAttribute("href");
                    var actualPage = source.QuerySelector("div.wp-pagenavi span.current")?.TextContent;
                    var lasTPage = source.QuerySelector("div.wp-pagenavi a.last")?.GetAttribute("href");
                    var finalizadosEl = source.QuerySelectorAll(".container #page ul.nav-tabs li a");
                    var finalizedPage = "";
                    foreach (var el in finalizadosEl) {
                        if (el.TextContent.Contains("Completos")) {
                            finalizedPage = el.GetAttribute("href");
                        }
                    }
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Preparando para retornar mangas!"));
                    foreach (var hq in hqsEl) {
                        var title = hq.QuerySelector("h3")?.TextContent;
                        var img = hq.QuerySelector("a img")?.GetAttribute("src");
                        var link = hq.QuerySelector("h3 a")?.GetAttribute("href");
                        var manga = new Hq { Link = link, Title = title, CoverSource = img };
                        hqs.Add(manga);
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, manga, $"{title} Adicionado"));
                    }
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                    return new LibraryPage {
                        Hqs = hqs, NextPage = nextPage, ActualPage = linkPage,
                        LastPage = lasTPage, FinalizedPage = finalizedPage
                    };
                }
                throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
            }
        }

        public override Hq GetHqInfo(string link) {
            lock (lock1) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                var source = htmlHelper.GetSourceCodeFromUrl(link);
                var hqInfo = new Hq();
                if (source != null) {
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
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Salvo em DB"));
                    return hqInfo;

                }
                throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
            }
        }

        public override Chapter GetChapterInfo(string link) {
            lock (lock2) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                IDocument leitor = htmlHelper.GetSourceCodeFromUrl(link);
                var chapter = new Chapter();
                if (leitor != null) {
                    var chapterTitle = leitor.QuerySelector("div.breadcrumb ul.container  li.active span")?.TextContent;
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {chapterTitle}"));
                    chapter.Title = chapterTitle;
                    chapter.Link = link;
                    var pages = GetPageList(leitor);
                    if (pages.Count <= 2) {
                        usingIe = true;
                        var Ie = browserHelper.GetInternetExplorer(link);
                        var doc = Ie.Document as HTMLDocument;
                        leitor = htmlHelper.GetSourceCodeFromHtml(doc.body.outerHTML);
                        pages = GetPageList(leitor);
                        Ie.Quit();
                        usingIe = false;
                    }
                    chapter.Pages = pages;
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                    return chapter;
                }
                throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
            }
            throw new NotImplementedException();
        }

        public List<Chapter> GetListChapters(IDocument hqSource) {
            lock (lock3) {
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
                    if (chapters != null) {
                        foreach (var chapterEl in chapters) {
                            var chapter = htmlHelper.GetSourceCodeFromHtml(chapterEl.GetAttribute("data-content"));
                            var linkEl = chapter.QuerySelector("a");
                            var chapterLink = linkEl?.GetAttribute("href");
                            var chapterTitle = $"Capitulo - {chapterEl.TextContent}";
                            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Adicionando {chapterTitle}"));
                            var chap = new Chapter { Link = chapterLink, Title = chapterTitle };
                            chapterList.Add(chap);
                        }
                    }
                }
                return chapterList;
            }
        }

        public List<Models.Page> GetPageList(IDocument chapterSoure) {
            lock (lock4) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando páginas"));
                var pageList = new List<Models.Page>();
                var pages = GetPageListFromScript(chapterSoure);
                var MangaPages = pages.QuerySelectorAll("img");
                if (MangaPages != null) {
                    var num = 1;
                    foreach (var img in MangaPages) {
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Adicionando página {num}"));
                        var page = new Page { Number = num, Source = img.GetAttribute("src") };
                        pageList.Add(page);
                        num++;
                    }
                }
                return pageList;
            }
        }

        private IDocument GetPageListFromScript(IDocument source) {
            lock (lock5) {
                var element = "";
                var imageElement = source.QuerySelector(".read-slideshow a");
                if (imageElement != null) {
                    var openTag = "<div class='pages-content'>";
                    var closeTag = "</div>";
                    var content = imageElement.ParentElement?.InnerHtml;
                    var html = source.Body.InnerHtml;
                    if (usingIe) {
                        element = $"{openTag}{content}{closeTag}";
                    } else {
                        html = html.Substring(html.IndexOf("var images = ["));
                        html = html.Substring(html.IndexOf("["), html.IndexOf("];"));
                        var otherPages = html.Split(']')[0];
                        otherPages = otherPages.Split('[')[1];
                        otherPages = otherPages.Replace("\"", "");
                        element = $"{openTag}{content}{otherPages}{closeTag}";
                    }

                    element = element.Replace("\"", "'");
                    element = element.Replace(".webp", "");
                    element = element.Replace("/images", "/mangas_files");
                } else if (source.QuerySelector(".image-content a") != null) {
                    var openTag = "<div class='pages-content'>";
                    var closeTag = "</div>";
                    var images = "";
                    var html = source.Body.InnerHtml;
                    html = html.Substring(html.IndexOf("var pages = ["));
                    html = html.Substring(html.IndexOf("["), html.IndexOf("];"));
                    var otherPages = html.Split(']')[0];
                    otherPages = otherPages.Split('[')[1];
                    otherPages = otherPages.Replace("\"", "");
                    var linkPages = otherPages.Split(new string[] { "url:" }, StringSplitOptions.None);
                    foreach (var linkPage in linkPages) {
                        var el = linkPage.Split(',')[0];
                        if (!el.Contains("id")) {
                            var link = el.Replace("}", "");
                            link = link.Replace("'", "");
                            link = link.Replace("\\", "");
                            images += $"<img src='{link}' />";
                        }
                    }
                    element = $"{openTag}{images}{closeTag}";
                    element = element.Replace("\"", "'");
                    element = element.Replace(".webp", "");
                    element = element.Replace("/images", "/mangas_files");
                }

                return htmlHelper.GetSourceCodeFromHtml(element);
            }
        }
    }
}
