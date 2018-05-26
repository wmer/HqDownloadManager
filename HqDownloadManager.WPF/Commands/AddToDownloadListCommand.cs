using DependencyInjectionResolver;
using HqDownloadManager.Download;
using HqDownloadManager.WPF.Helpers;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class AddToDownloadListCommand : CommandBase<Hq> {
        private DownloadManager _downloadManager;
        private DownloadHelper _downloadHelper;

        public AddToDownloadListCommand(
                        DownloadManager downloadManager, 
                        DownloadHelper downloadHelper) {
            _downloadManager = downloadManager;
            _downloadHelper = downloadHelper;
        }

        public override void Execute(Hq parameter) {
            var location = _downloadHelper.SelectFolder();
            _downloadManager.AddToDownloadList(parameter, location);
        }
    }
}
