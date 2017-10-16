using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Helpers;
using AngleSharp.Dom;
using HqDownloadManager.Core.CustomEventArgs;

namespace HqDownloadManager.Core.Sources {
    internal class UnionMangasSource : HqSource {
        private HtmlSourceHelper htmlHelper;
        private object lock1 = new object();
        private object lock2 = new object();
        private object lock3 = new object();
        private object lock4 = new object();
        private object lock5 = new object();
        private object lock6 = new object();

        public UnionMangasSource(HtmlSourceHelper htmlHelper) {
            this.htmlHelper = htmlHelper;
        }

        public override List<Hq> GetUpdates(String updatePage) {
            lock (lock6) {
                var source = htmlHelper.GetSourceCodeFromUrl(updatePage);
                var hqs = new List<Hq>();
                if (source != null) {
                    return hqs;
                }
                throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
            }
        }

        public override LibraryPage GetLibrary(String linkPage) {
            lock (lock5) {
                throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
            }
        }

        public override Hq GetHqInfo(string link) {
            lock (lock1) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                var source = htmlHelper.GetSourceCodeFromUrl(link);
                var hqInfo = new Hq();
                if (source != null) {
                    var title = source.QuerySelector(".tamanho-bloco-perfil h2")?.TextContent;
                    var img = source.QuerySelector("img.img-thumbnail");
                    var synopsis = source.QuerySelector(".tamanho-bloco-perfil div.panel-body")?.TextContent;
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {title}"));
                    hqInfo.Title = title;
                    hqInfo.CoverSource = img?.GetAttribute("src");
                    hqInfo.Link = link;
                    hqInfo.Synopsis = synopsis.Replace("\n", "").Trim();
                    var hqInfos = source.QuerySelectorAll(".tamanho-bloco-perfil h4.manga-perfil");
                    foreach (var info in hqInfos) {
                        if (info.TextContent.Contains("Autor")) {
                            hqInfo.Author = info.TextContent.Replace("Autor:", "").Trim();
                        }
                    }
                    hqInfo.Chapters = GetListChapters(source).Reverse<Chapter>().ToList();
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto!"));
                    return hqInfo;
                }
                throw new Exception("Ocorreu um erro ao buscar informaçoes da Hq");
            }
        }

        public override Chapter GetChapterInfo(string link) {
            lock (lock2) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Pegando dados da página"));
                var leitor = htmlHelper.GetSourceCodeFromUrl(link);
                var chapter = new Chapter();
                if (leitor != null) {
                    var titles = leitor.QuerySelectorAll(".text-left h2");
                    var chapterTitle = $"{titles?[0]?.TextContent} - {titles?[1]?.TextContent}";
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando informações de {chapterTitle}"));
                    chapter.Title = chapterTitle;
                    chapter.Link = link;
                    chapter.Pages = GetPageList(leitor);
                    OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Tudo pronto"));
                    return chapter;
                }
                throw new Exception("Ocorreu um erro ao buscar informaçoes do capitulo");
            }
        }

        private List<Chapter> GetListChapters(IDocument hqSource) {
            lock (lock3) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando capitulos"));
                var chapterList = new List<Chapter>();
                var chapters = hqSource.QuerySelectorAll(".lancamento-linha div:nth-child(1) a");
                if (chapters != null) {
                    foreach (var chapter in chapters) {
                        var chapterLink = chapter.GetAttribute("href");
                        var chapterTitle = $"Capitulo - {chapter.TextContent.Replace("Cap. ", "")}";
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Adicionando {chapterTitle}"));
                        chapterList.Add(new Chapter { Link = chapterLink, Title = chapterTitle });
                    }
                }
                return chapterList;
            }
        }

        public List<Page> GetPageList(IDocument chapterSoure) {
            lock (lock4) {
                OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Buscando páginas"));
                var pageList = new List<Page>();
                var pages = chapterSoure.QuerySelectorAll(".item img.real:nth-child(1)");
                if (pages != null) {
                    var num = 1;
                    foreach (var page in pages) {
                        var imgLink = page.GetAttribute("data-lazy");
                        OnProcessingProgress(new ProcessingEventArgs(DateTime.Now, $"Adicionando página {num}"));
                        pageList.Add(new Page { Number = num, Source = imgLink });
                        num++;
                    }
                }
                return pageList;
            }
        }
    }
}
