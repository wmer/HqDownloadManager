using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace HqDownloadManager.Core.Sources {
    public class MangasProjectSource : HqSource {
        private string BaseAdress { get; set; }

        public MangasProjectSource(HtmlSourceHelper htmlHelper, BrowserHelper browserHelper) : base(htmlHelper, browserHelper) {
        }

        public override async Task<Chapter> GetChapterInfo(string link) {
            try {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Abrindo Internet Explorer"));
                var driver = await BrowserHelper.GetPhantomJSMobile(link);
                var pageSource = driver.PageSource;
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                IDocument leitor = HtmlHelper.GetSourceCodeFromHtml(pageSource);
                var chapter = new Chapter();

                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                if (leitor == null) {
                    driver.Quit();
                    throw new Exception("Ocorreu um erro ao buscar informaçoes do capitulo");
                }
                var titleChap = leitor.QuerySelector("div.barra-titulo div.title-container div.title");
                var title = titleChap?.QuerySelector("span.name")?.TextContent;
                var chap = titleChap?.QuerySelector("span.chap")?.TextContent;
                var chapterTitle = $"{title} - {chap}";
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {chapterTitle}"));
                chapter.Title = chapterTitle;
                chapter.Link = link;
                List<Page> pageList = GetPageList(driver);
                if (pageList.Count() > 1) {
                    chapter.Pages = pageList;
                } else {
                    driver.Quit();
                    throw new Exception("Ocorreu um erro ao buscar informaçoes do capitulo");
                }
                driver.Quit();
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                return chapter;
            } catch (Exception e) {
                OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, link, e));
                return null;
            }
        }

        public override async Task<Hq> GetHqInfo(string link) {
            try {
                Uri site = new Uri(link);
                BaseAdress = $"{site.Scheme}://{site.Host}";

                var driver = await BrowserHelper.GetPhantomJS(link);
                //Task.Delay(5000).Wait();
                var pageSource = driver.PageSource;
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                IDocument source = HtmlHelper.GetSourceCodeFromHtml(pageSource);
                driver.Quit();

                var hqInfo = new Hq();

                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                if (source == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
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
                var lastchapter = source.QuerySelector("#chapter-list .list-of-chapters li a");
                var chap = await GetListChapters($"{BaseAdress}{lastchapter?.GetAttribute("href")}");
                hqInfo.Chapters = chap.Reverse<Chapter>().ToList();

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
                Uri site = new Uri(linkPage);
                BaseAdress = $"{site.Scheme}://{site.Host}";
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Abrindo Internet Explorer"));
                var driver = await BrowserHelper.GetPhantomJS(linkPage);
                var pageSource = driver.PageSource;
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                IDocument source = HtmlHelper.GetSourceCodeFromHtml(pageSource);
                driver.Quit();
                var hqs = new List<Hq>();
                if (source == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
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
                var lethers = source.QuerySelectorAll("#az-order div a");
                var letherLink = new Dictionary<string, string>();
                foreach (var lether in lethers) {
                    letherLink[lether.TextContent.Trim()] = $"{BaseAdress}{lether.GetAttribute("href")}";
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
                        if (!string.IsNullOrEmpty(link)) {
                            var manga = new Hq { Link = $"{BaseAdress}{link}", Title = title, CoverSource = img };
                            hqs.Add(manga);
                            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, manga, $"{title} Adicionado"));
                        }
                    }
                }
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                return new LibraryPage {
                    Hqs = hqs, NextPage = $"{BaseAdress}{nextPage}",
                    FinalizedPage = $"{BaseAdress}{finalizedPage}", Letras = letherLink
                };
            } catch (Exception e) {
                OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, linkPage, e));
                return null;
            }
        }

        public override async Task<List<Hq>> GetUpdates(string updatePage) {
            try {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando Lançamentos..."));
                Uri site = new Uri(updatePage);
                BaseAdress = $"{site.Scheme}://{site.Host}";
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Abrindo Internet Explorer"));
                var driver = await BrowserHelper.GetPhantomJS(updatePage);
                var loadcMore = driver.FindElement(By.CssSelector("a.loadmore"));
                loadcMore.Click();
                Task.Delay(500).Wait();
                loadcMore.Click();
                Task.Delay(500).Wait();
                var pageSource = driver.PageSource;
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                IDocument source = HtmlHelper.GetSourceCodeFromHtml(pageSource);
                driver.Quit();
                var hqs = new List<Hq>();
                if (source == null) throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
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
                    if (title == null) continue;
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados de {title}"));
                    var link = hq.QuerySelector(".lancamento-desc a")?.GetAttribute("href");
                    if (!string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(img)) {
                        var update = new Hq { Link = $"{BaseAdress}{link}", Title = title, CoverSource = img };
                        hqs.Add(update);
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, update, $"{title} Adicionado"));
                    }
                }
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                return hqs;
            } catch (Exception e) {
                OnProcessingProgressError(new ProcessingErrorEventArgs(DateTime.Now, updatePage, e));
                return null;
            }
        }

        public async Task<List<Chapter>> GetListChapters(String link) {
            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando capitulos"));
            var chapterList = new List<Chapter>();
            var driver = await BrowserHelper.GetPhantomJS(link);
            var pageSource = driver.PageSource;
            OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
            IDocument leitor = HtmlHelper.GetSourceCodeFromHtml(pageSource);
            driver.Quit();

            if (leitor == null) return chapterList;
            var chapters = leitor.QuerySelectorAll("#chapter-selection-dropdown div.selection div a");
            if (chapters != null) {
                foreach (var chapter in chapters) {
                    var chapterLink = $"{BaseAdress}{chapter.GetAttribute("href")}";
                    var chapterTitle = $"Capitulo - {chapter.QuerySelector("span.chapter")?.TextContent.Replace("Cap. ", "")}";
                    chapterList.Add(new Chapter { Title = chapterTitle, Link = chapterLink });
                }
            }

            return chapterList;
        }

        public List<Page> GetPageList(RemoteWebDriver document) {
            lock (Lock1) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando páginas"));
                IDocument leitor = HtmlHelper.GetSourceCodeFromHtml(document.PageSource);
                var pageList = new List<Page>();

                if (leitor == null) throw new Exception("Ocorreu um erro ao buscar informaçoes do capitulo");
                var pages = leitor.QuerySelectorAll("div#manga div.manga-pages img.page-image");

                var num = 1;
                foreach (var img in pages) {
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
