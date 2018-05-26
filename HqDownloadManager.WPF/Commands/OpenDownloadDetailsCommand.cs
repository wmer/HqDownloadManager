using DependencyInjectionResolver;
using HqDownloadManager.Helpers;
using HqDownloadManager.Models;
using HqDownloadManager.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.Navigation;

namespace HqDownloadManager.WPF.Commands {
    public class OpenDownloadDetailsCommand : CommandBase<DownloadedHq> {
        private DownloadHelper _downloadHelper;
        private NavigationManager _navigationManager;

        public OpenDownloadDetailsCommand(
                        DownloadHelper downloadHelper, 
                        NavigationManager navigationManager) {
            _downloadHelper = downloadHelper;
            _navigationManager = navigationManager;
        }
        public override void Execute(DownloadedHq parameter) {
            parameter.Chapters = _downloadHelper.GetChapters(parameter);
            _navigationManager.Navigate<DownloadDetailsPage>(parameter.Hq.Title, parameter);
        }
    }
}
