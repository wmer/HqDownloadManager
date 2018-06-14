using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.WPF.Helpers;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.UserControls.Commands {
    public class AddToDownloadListCommand : CommandBase<Hq> {
        private DownloadManager _downloadManager;
        private DownloadHelper _downloadHelper;
        private DownloadManagerContext _downloadContext;

        public AddToDownloadListCommand(
                        DownloadManager downloadManager,
                        DownloadHelper downloadHelper,
                        DownloadManagerContext downloadContext) {
            _downloadManager = downloadManager;
            _downloadHelper = downloadHelper;
            _downloadContext = downloadContext;
        }

        public override bool CanExecute(Hq parameter) =>
                                            parameter is Hq hq &&
                                            hq.Chapters != null &&
                                            hq.Chapters.Count > 0;

        public override void Execute(Hq parameter) {
            var locais = _downloadContext.DownloadLocation.FindAll();
            var location = "";
            if (locais != null && locais.Count() > 0) {
                var lastLocation = locais.LastOrDefault();
                string messageBoxText = $"Salvar em: {lastLocation.Location}?";
                string caption = "Download";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                switch (result) {
                    case MessageBoxResult.Yes:
                        location = lastLocation.Location;
                        break;
                    case MessageBoxResult.No:
                        location = _downloadHelper.SelectFolder();
                        break;
                }
            } else {
                location = _downloadHelper.SelectFolder();
            }
            _downloadManager.AddToDownloadList(parameter, location);
        }
    }
}
