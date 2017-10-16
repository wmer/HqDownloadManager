using DependencyInjectionResolver;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Helpers {
    public class Teste {
        public void AddInDB() {
            var dataContext = new DatabaseContext();
            var sourceManager = new DependencyInjection().Resolve<SourceManager>();

            var updates = sourceManager.GetUpdates("https://mangashost.net/");
            foreach (var hq in updates) {
                var manga = sourceManager.GetInfo(hq.Link) as Hq;
                if (manga.Chapters.Count <= 5) {
                    dataContext.Hqs.Save(manga);
                    foreach (var chapter in manga.Chapters) {
                        var chap = sourceManager.GetInfo(chapter.Link) as Chapter;
                        chap.Hq = manga;
                        dataContext.Chapters.Save(chap);
                        foreach (var page in chap.Pages) {
                            page.Chapter = chap;
                            dataContext.Pages.Save(page);
                        }
                    }
                }
            }

        }
    }
}
