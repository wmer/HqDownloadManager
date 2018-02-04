using DependencyInjectionResolver;
using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Controller.ViewModel.ComboBox;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Controller.ViewModel.SourceUpdate;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqManager;
using HqManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controller {
    public class SourceUpdateController : SourceControllerBase {
        protected SourceUpdateViewModel _sourceUpdate;

        public SourceUpdateController(
                    SourceManager sourceManager,
                    ControlsHelper controllerHelper,
                    DownloadManager downloadManager,
                    EntryManager entryManage):base(
                        sourceManager, 
                        entryManage, 
                        downloadManager, 
                        controllerHelper) {}

        public override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            _sourceUpdate = _controllerHelper.FindResource<SourceUpdateViewModel>("Updates");
            ShowUpdates();
        }

        public override void HqStatusChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedStatus" && _hqStatusView.SelectedStatus != "Add to...") {
                _entryManager.AddTo(_hqStatusView.ReadStatus, _sourceUpdate.SelectedUpdate.Hq);
            }
        }

        public override void SelectedSourceChanged(object sender, PropertyChangedEventArgs e) {
            base.SelectedSourceChanged(sender, e);
            ShowUpdates();
        }

        public void Downloadupdates() {
            var hq = _hqStatusView.Hq;
            hq.Chapters = _sourceUpdate.SelectedUpdate.Chapters;
            _entryManager.AddTo(ReadStatus.LENDO, hq);
            _downloadManager.AddToDownloadList(hq, $"{CoreConfiguration.DownloadLocation}\\Hqs");
        }

        private void ShowUpdates() {
            Task.Run(async () => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _notification.Visibility = Visibility.Visible;
                });
                var updates = new List<Update>();
                _actualSource.GetUpdates(out updates);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _sourceUpdate.Updates = new ObservableCollection<Update>(updates);
                    _notification.Visibility = Visibility.Collapsed;
                });
            });
        }
    }
}
