using DependencyInjectionResolver;
using HqDownloadManager.Download;
using HqDownloadManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class StartDownloadCommand : CommandBase {
        private DownloadManager _downloadManager;

        public StartDownloadCommand(DownloadManager downloadManager) {
            _downloadManager = downloadManager;
        }

        public override bool CanExecute(object parameter) => 
                                        parameter is DownloadListViewModel downloadListViewModel && 
                                        downloadListViewModel.DownloadList != null && 
                                        downloadListViewModel.DownloadList.Count() > 0;

        public override void Execute(object parameter) =>
                                        _downloadManager.Download();
    }
}
