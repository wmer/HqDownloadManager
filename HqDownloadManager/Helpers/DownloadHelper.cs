using HqDownloadManager.Database;
using HqDownloadManager.Models;
using MangaScraping.Events;
using MangaScraping.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HqDownloadManager.Helpers {
    public class DownloadHelper {
        private readonly DownloadManagerContext _downloadContext;
        private object _lock1 = new object();

        public DownloadHelper(DownloadManagerContext downloadContext) {
            _downloadContext = downloadContext;
        }

        public void SaveDownloadedHq(DownloadedHq downloadedHq) {
            lock (_lock1) {
                downloadedHq.Hq.Chapters = null;
                var fileLoc = $"{downloadedHq.Location}\\{downloadedHq.Hq.Title}.json";
                if (!File.Exists(fileLoc)) {
                    using (StreamWriter file = File.CreateText(fileLoc)) {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, downloadedHq);
                    }
                }
            }
        }

        public List<DownloadedHq> GetDownloads() {
            //_downloadContext.DownloadLocation.Save(new DownloadLocation { Location = "D:\\Mangas" });
            CoreEventHub.OnProcessingStart(this, null);
            var list = new List<DownloadedHq>();
            CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, "Buscando Locais"));
            var locais = _downloadContext.DownloadLocation.FindAll();
            if (locais != null && locais.Count() > 0) {
                CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, "Buscando Hqs"));
                foreach (var local in locais) {
                    var directory = new DirectoryInfo(local.Location);
                    var mangas = directory.GetDirectories();
                    foreach (var manga in mangas) {
                        DownloadedHq downloadedHq = new DownloadedHq();
                        var json = manga.GetFiles("*.json").FirstOrDefault();
                        if (json != null) {
                            using (StreamReader file = json.OpenText()) {
                                JsonSerializer serializer = new JsonSerializer();
                                downloadedHq = serializer.Deserialize(file, typeof(DownloadedHq)) as DownloadedHq;
                            }
                        }
                        if (downloadedHq != null) {
                            list.Add(downloadedHq);
                        }
                    }
                }
            }

            CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, "Tudo Pronto!"));
            CoreEventHub.OnProcessingEnd(this, null);
            return list;
        }

        public List<DownloadedChapter> GetChapters(DownloadedHq downloadedHq) {
            CoreEventHub.OnProcessingStart(this, null);
            var chapters = new List<DownloadedChapter>();
            var dir = new DirectoryInfo(downloadedHq.Location);
            var paths = dir.GetDirectories();
            CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, $"Buscando Capitulos de {downloadedHq.Hq.Title}"));
            foreach (var path in paths) {
                CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, $"Adicioandn capitulo {path.Name}"));
                var title = path.Name;
                var fisrt = false;
                title = Regex.Replace(title, @"\b(\d+)", new MatchEvaluator((match) => {
                    var m = match.Value;
                    if (!fisrt) {
                        m = m.PadLeft(3, '0');
                        fisrt = true;
                    }
                    return m;
                }));
                var chap = new Chapter { Title = title };
                var downlodeadChapter = new DownloadedChapter { Date = path.CreationTime, Location = path.FullName, Chapter = chap };
                chapters.Add(downlodeadChapter);
            }

            chapters = chapters.OrderBy(x => x.Chapter.Title).ToList<DownloadedChapter>();
            CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, "Tudo Pronto!"));
            CoreEventHub.OnProcessingEnd(this, null);
            return chapters;
        }

        public List<Page> GetPages(string location) {
            CoreEventHub.OnProcessingStart(this, null);
            CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, "Buscando Páginas"));
            var pages = new List<Page>();
            if (!string.IsNullOrEmpty(location)) {
                var dir = new DirectoryInfo(location);
                var images = dir.GetFiles();
                foreach (var image in images) {
                    var resultString = Regex.Match(image.Name, @"\d+").Value;
                    var number = Int32.Parse(resultString);
                    CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, $"Adicionando página {number}"));
                    var pg = new Page { Number = number, Source = image.FullName };
                    pages.Add(pg);
                }
            }

            CoreEventHub.OnProcessingProgress(this, new ProcessingEventArgs(DateTime.Now, "Tudo Pronto!"));
            CoreEventHub.OnProcessingEnd(this, null);
            return pages;
        }

        public void DeleteManga(DownloadedHq downloadedHq) =>
                                Directory.Delete(downloadedHq.Location, true);
    }
}
