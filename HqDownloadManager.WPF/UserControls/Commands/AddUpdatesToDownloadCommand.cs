﻿using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.WPF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.UserControls.Commands {
    public class AddUpdatesToDownloadCommand : CommandBase<DetailsUserControl> {
        private DownloadManager _downloadManager;
        private DownloadHelper _downloadHelper;
        private DownloadManagerContext _downloadContext;

        public AddUpdatesToDownloadCommand(
                        DownloadManager downloadManager,
                        DownloadHelper downloadHelper,
                        DownloadManagerContext downloadContext) {
            _downloadManager = downloadManager;
            _downloadHelper = downloadHelper;
            _downloadContext = downloadContext;
        }

        public override bool CanExecute(DetailsUserControl parameter) =>
                            parameter.Update != null &&
                            parameter.Update.Chapters != null &&
                            parameter.Update.Chapters.Count > 0;

        public override void Execute(DetailsUserControl parameter) {
            var locais = _downloadContext.DownloadLocation.FindAll();
            var location = "";
            var hq = parameter.Hq;
            hq.Chapters = parameter.Update.Chapters;
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
            _downloadManager.AddToDownloadList(hq, location);
        }
    }
}
