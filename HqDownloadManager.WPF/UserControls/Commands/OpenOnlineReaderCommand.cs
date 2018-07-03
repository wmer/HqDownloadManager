using HqDownloadManager.Models;
using HqDownloadManager.WPF.Views;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.Navigation;

namespace HqDownloadManager.WPF.UserControls.Commands {
    public class OpenOnlineReaderCommand : CommandBase<DetailsUserControl> {
        private readonly NavigationManager _navigationManager;

        public OpenOnlineReaderCommand(NavigationManager navigationManager) {
            _navigationManager = navigationManager;
        }

        public override bool CanExecute(DetailsUserControl parameter) => parameter is DetailsUserControl details &&
                                                                         details.Hq != null &&
                                                                         details.Hq.Chapters != null &&
                                                                         details.SelectedChapter != null;

        public override void Execute(DetailsUserControl parameter) {
            var downloadedHq = new DownloadedHq { Hq = parameter.Hq, Chapters = new List<DownloadedChapter>() };
            var downloadedChapter = new DownloadedChapter { Chapter = parameter.SelectedChapter };
            if (downloadedHq.Hq != null && downloadedHq.Hq.Chapters != null) {
                foreach (var chap in downloadedHq.Hq.Chapters) {
                    var dwChapter = new DownloadedChapter { Chapter = chap };
                    downloadedHq.Chapters.Add(dwChapter);
                }
            }

            _navigationManager.Navigate<ReaderPage>(parameter.SelectedChapter.Title,
                                                                      downloadedHq,
                                                                      downloadedChapter);
        }
    }
}
