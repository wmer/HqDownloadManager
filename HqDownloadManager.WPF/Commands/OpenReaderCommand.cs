using HqDownloadManager.Helpers;
using HqDownloadManager.WPF.Databases;
using HqDownloadManager.WPF.Models;
using HqDownloadManager.WPF.ViewModels;
using HqDownloadManager.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.Navigation;

namespace HqDownloadManager.WPF.Commands {
    public class OpenReaderCommand : CommandBase<DownloadDetailsViewModel> {
        private readonly NavigationManager _navigationManager;

        public OpenReaderCommand(NavigationManager navigationManager) {
            _navigationManager = navigationManager;
        }

        public override bool CanExecute(DownloadDetailsViewModel parameter) =>
                                                    parameter is DownloadDetailsViewModel dwDetails &&
                                                    dwDetails.SelectedChapter != null &&
                                                    parameter.SelectedChapter.Chapter != null &&
                                                    dwDetails.DownloadedHq != null &&
                                                    dwDetails.DownloadedHq.Hq != null;

        public override void Execute(DownloadDetailsViewModel parameter) =>
                      _navigationManager.Navigate<ReaderPage>(parameter.SelectedChapter.Chapter.Title,
                                                                                parameter.DownloadedHq,
                                                                                parameter.SelectedChapter);
    }
}
