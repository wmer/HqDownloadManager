using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Helpers;
using AngleSharp.Dom;
using System.Threading;
using HqDownloadManager.Core.CustomEventArgs;
using MSHTML;
using HqDownloadManager.Core.Database;

namespace HqDownloadManager.Core.Sources {
    internal class MangasProjectSource : HqSource {
        private string BaseAdress { get; set; }
        public HtmlSourceHelper htmlHelper;
        public BrowserHelper browserHelper;
        private object lock1 = new object();
        private object lock2 = new object();
        private object lock3 = new object();
        private object lock4 = new object();
        private object lock5 = new object();
        private object lock6 = new object();
        private object lock7 = new object();
        private object lock8 = new object();
        private object lock9 = new object();

        public MangasProjectSource(LibraryContext libraryContext, HtmlSourceHelper htmlHelper, BrowserHelper browserHelper) : base(libraryContext) {
            this.htmlHelper = htmlHelper;
            this.browserHelper = browserHelper;
        }

        public override List<Hq> GetUpdates(String updatePage) {
            lock (lock1) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando Lançamentos..."));
                Uri site = new Uri(updatePage);
                BaseAdress = $"{site.Scheme}://{site.Host}";
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Abrindo Internet Explorer"));
                var Ie = browserHelper.GetInternetExplorer(updatePage);
                HTMLDocument doc = Ie.Document as HTMLDocument;
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                var links = doc.getElementsByTagName("a");
                foreach (IHTMLElement link in links) {
                    if (link.className == "loadmore") {
                        link.click();
                        break;
                    }
                }
                IDocument source = htmlHelper.GetSourceCodeFromHtml(doc.body.outerHTML);
                Ie.Quit();
                var hqs = new List<Hq>();
                if (source != null) {
                    var hqsEl = source.QuerySelectorAll("div#recent_releases li");
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"{hqsEl.Count()} mangas encontrados!"));
                    foreach (var hq in hqsEl) {
                        var title = hq.QuerySelector(".lancamento-title")?.TextContent;
                        var imgEl = hq.QuerySelector(".cover-image")?.GetAttribute("style");
                        imgEl = imgEl?.Replace("background-image: url(", "");
                        imgEl = imgEl?.Replace(")", "");
                        imgEl = imgEl?.Replace("'", "");
                        imgEl = imgEl?.Replace("\"", "");
                        var img = imgEl?.Replace(";", "");
                        if (title != null) {
                            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados de {title}"));
                            var link = hq.QuerySelector(".lancamento-desc a")?.GetAttribute("href");
                            var update = new Hq { Link = $"{BaseAdress}{link}", Title = title, CoverSource = img };
                            hqs.Add(update);
                            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, update, $"{title} Adicionado"));
                        }
                    }
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                    return hqs;
                }
                throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
            }
        }

        public override LibraryPage GetLibrary(String linkPage) {
            lock (lock2) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, "Processando os Mangas..."));
                Uri site = new Uri(linkPage);
                BaseAdress = $"{site.Scheme}://{site.Host}";
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Abrindo Internet Explorer"));
                var Ie = browserHelper.GetInternetExplorer(linkPage);
                var doc = Ie.Document as HTMLDocument;
                IDocument source = htmlHelper.GetSourceCodeFromHtml(doc.body.outerHTML);
                Ie.Quit();
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                var hqs = new List<Hq>();
                if (source != null) {
                    var hqsEl = source.QuerySelectorAll(".content-wraper ul.seriesList li");
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"{hqsEl.Count()} mangas encontrados!"));
                    var nextPage = source.QuerySelector("ul.content-pagination li.next a")?.GetAttribute("href");
                    var finalizadosEl = source.QuerySelectorAll("#subtopnav ul#menu-titulos li a");
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando links"));
                    var finalizedPage = "";
                    foreach (var el in finalizadosEl) {
                        var test = el.TextContent;
                        if (el.TextContent.Contains("Recém Finalizados")) {
                            finalizedPage = el.GetAttribute("href");
                        }
                    }
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Preparando para retornar mangas!"));
                    foreach (var hq in hqsEl) {
                        var title = hq.QuerySelector(".series-title h1")?.TextContent;
                        var imgEl = hq.QuerySelector(".cover-image")?.GetAttribute("style");
                        imgEl = imgEl?.Replace("background-image: url(", "");
                        imgEl = imgEl?.Replace(")", "");
                        imgEl = imgEl?.Replace("'", "");
                        imgEl = imgEl?.Replace("\"", "");
                        var img = imgEl?.Replace(";", "");
                        var link = hq.QuerySelector("a")?.GetAttribute("href");
                        if (title != null) {
                            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados de {title}"));
                            if (!img.Contains(site.Scheme)) {
                                img = $"{BaseAdress}{img}";
                            }
                            var manga = new Hq { Link = $"{BaseAdress}{link}", Title = title, CoverSource = img };
                            hqs.Add(manga);
                            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, manga, $"{title} Adicionado"));
                        }
                    }
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                    return new LibraryPage {
                        Hqs = hqs, NextPage = $"{BaseAdress}{nextPage}", ActualPage = linkPage, FinalizedPage = $"{BaseAdress}{finalizedPage}"
                    };
                }
                throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
            }
        }

        public override Hq GetHqInfo(string link) {
            lock (lock3) {
                Uri site = new Uri(link);
                BaseAdress = $"{site.Scheme}://{site.Host}";

                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Abrindo Internet Explorer"));
                var Ie = browserHelper.GetInternetExplorer(link);
                var doc = Ie.Document as HTMLDocument;
                IDocument source = htmlHelper.GetSourceCodeFromHtml(doc.body.outerHTML);
                var hqInfo = new Hq();
                Ie.Quit();

                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                if (source != null) {
                    var title = source.QuerySelector("div#series-data div.series-info span.series-title h1")?.TextContent;
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {title}"));
                    var img = source.QuerySelector("div.series-img div.cover img");
                    var author = source.QuerySelector("div#series-data div.series-info span.series-author")?.TextContent;
                    var synopsis = source.QuerySelector("div#series-data div.series-info span.series-desc")?.TextContent;
                    hqInfo.Title = title;
                    hqInfo.Author = author.Replace("\n", "").Trim();
                    hqInfo.CoverSource = img?.GetAttribute("src");
                    hqInfo.Synopsis = synopsis.Replace("\n", "").Trim();
                    hqInfo.Link = link;
                    var Lastchapter = source.QuerySelector(".list-of-chapters li a");
                    hqInfo.Chapters = GetListChapters($"{BaseAdress}{Lastchapter?.GetAttribute("href")}").Reverse<Chapter>().ToList();

                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto!"));
                    return hqInfo;
                }
                throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
            }
        }

        public override Chapter GetChapterInfo(string link) {
            lock (lock4) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Abrindo Internet Explorer"));
                var Ie = browserHelper.GetInternetExplorer(link);
                Task.Delay(5000).Wait();
                var adultSerie = IsAdultSerie((HTMLDocument)Ie.Document);
                if (adultSerie.Item1) {
                    IgnoreMessageOfAdultContent(adultSerie.Item2);
                }
                var doc = Ie.Document as HTMLDocument;
                var chapter = new Chapter();
                IDocument leitor = htmlHelper.GetSourceCodeFromHtml(doc.body.outerHTML);
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                if (leitor != null) {
                    var titleChap = leitor.QuerySelector("div.barra-titulo div.title-container div.title");
                    var title = titleChap?.QuerySelector("span.name")?.TextContent;
                    var chap = titleChap?.QuerySelector("span.chap")?.TextContent;
                    var chapterTitle = $"{title} - {chap}";
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {chapterTitle}"));
                    chapter.Title = chapterTitle;
                    chapter.Link = link;
                    List<Page> pageList = GetPageList((HTMLDocument)Ie.Document);
                    if (pageList.Count() > 1) {
                        chapter.Pages = pageList;
                    } else {
                        Ie.Quit();
                        throw new Exception("Ocorreu um erro ao buscar informaçoes do capitulo");
                    }
                    Ie.Quit();
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                    return chapter;
                }
                Ie.Quit();
                throw new Exception("Ocorreu um erro ao buscar informaçoes do capitulo");
            }
        }

        public List<Chapter> GetListChapters(String link) {
            lock (lock5) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando capitulos"));
                var chapterList = new List<Chapter>();
                var Ie = browserHelper.GetInternetExplorer(link);
                var doc = Ie.Document as HTMLDocument;
                IDocument leitor = htmlHelper.GetSourceCodeFromHtml(doc.body.outerHTML);
                Ie.Quit();

                if (leitor != null) {
                    var chapters = leitor.QuerySelectorAll("#chapter-selection-dropdown div.selection div a");
                    if (chapters != null) {
                        foreach (var chapter in chapters) {
                            var chapterLink = $"{BaseAdress}{chapter.GetAttribute("href")}";
                            var chapterTitle = $"Capitulo - {chapter.QuerySelector("span.chapter")?.TextContent.Replace("Cap. ", "")}";
                            chapterList.Add(new Chapter { Title = chapterTitle, Link = chapterLink });
                        }
                    }
                }

                return chapterList;
            }
        }

        public List<Page> GetPageList(HTMLDocument document) {
            lock (lock6) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando páginas"));
                IDocument leitor = htmlHelper.GetSourceCodeFromHtml(document.body.outerHTML);
                var pageList = new List<Page>();
                var divNextAndPage = GetDivPageAndDivNext(document);

                if (leitor != null) {
                    int numPages;
                    var num = int.TryParse(leitor.QuerySelector("#totalPages")?.TextContent, out numPages);
                    int propPage = numPages / 10;
                    numPages = numPages + propPage;
                    var child = (IHTMLElementCollection)divNextAndPage.DivPage.children;

                    for (var i = 0; i < numPages; i++) {
                        if (child != null && child.length > 0) {
                            foreach (IHTMLElement img in child) {
                                var tag = img.tagName;
                                if (img.tagName == "IMG") {
                                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Adicionando página {i + 1}"));
                                    Task.Delay(700).Wait();
                                    var src = (String)img.getAttribute("src");
                                    pageList.Add(new Page { Number = (i + 1), Source = src });
                                    divNextAndPage.DivNext.click();
                                }
                            }
                        } else {
                            throw new Exception("Ocorreu um erro ao buscar informaçoes do capitulo");
                        }
                    }
                    return pageList;
                }

                throw new Exception("Ocorreu um erro ao buscar informaçoes do capitulo");
            }
        }

        private (IHTMLElement DivPage, IHTMLElement DivNext) GetDivPageAndDivNext(HTMLDocument document) {
            lock (lock7) {
                IHTMLElement divPage = null;
                IHTMLElement divNext = null;
                var allDivs = document.getElementsByTagName("div");

                foreach (IHTMLElement div in allDivs) {
                    if (div.className == "page") {
                        divPage = div;
                    }
                    if (div.className == "page-next") {
                        divNext = div;
                    }
                    if (divPage != null && divNext != null) {
                        break;
                    }
                }
                return (divPage, divNext);
            }
        }

        private void IgnoreMessageOfAdultContent(IHTMLElement message) {
            lock (lock8) {
                message.click();
                Task.Delay(5000).Wait();
            }
        }

        private (bool, IHTMLElement) IsAdultSerie(HTMLDocument document) {
            lock (lock9) {
                (bool, IHTMLElement) check = (false, null);
                IHTMLElementCollection allLinks = document.getElementsByTagName("a");
                foreach (IHTMLElement Ellink in allLinks) {
                    if (Ellink.className == "eighteen-but") {
                        check = (true, Ellink);
                        break;
                    }
                }
                return check;
            }
        }
    }
}
