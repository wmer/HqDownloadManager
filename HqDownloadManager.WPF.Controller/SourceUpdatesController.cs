using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqDownloadManager.Download.Configuration;
using HqDownloadManager.Shared.Database;
using HqDownloadManager.Shared.ViewModel.Reader;
using HqDownloadManager.Shared.ViewModel.SourceUpdate;
using HqManager;
using HqManager.Models;
using WPF.Tools.Navigation;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Controller {
    public class SourceUpdatesController : SourceControllerBase {
        protected SourceUpdateViewModel _sourceUpdate;

        public SourceUpdatesController(
              SourceManager sourceManager,
              ConfigurationContext configurationContext,
              DownloadManager downloadManager,
              EntryManager entryManage,
              NavigationManager navigationManager) : base(
                  sourceManager,
                  configurationContext,
                  entryManage,
                  downloadManager,
                  navigationManager) { }

        public override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            _sourceUpdate = ControlsHelper.FindResource<SourceUpdateViewModel>("Updates");
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
            _downloadManager.AddToDownloadList(hq, DownloadConfiguration.DownloadLocations[0]);
        }

        private void ShowUpdates() {
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    _notification.Visibility = true;
                });
                var updates = new List<Update>();
                _actualSource.GetUpdates(out updates);
                Dispatcher.Invoke(() => {
                    _sourceUpdate.Updates = new ObservableCollection<Update>(updates);
                    _notification.Visibility = false;
                });
            });
        }

        public void ReadUpdate<T>(Chapter chapter) where T : System.Windows.Controls.Page {
            var hq = _detailsViewModel.Hq;
            hq.Chapters = _sourceUpdate.SelectedUpdate.Chapters;
            var readerModel = new ReaderViewModel {
                Hq = hq,
                ActualChapter = chapter,
                ActualChapterIndex = _sourceUpdate.SelectedUpdate.Chapters.IndexOf(chapter)
            };
            var next = readerModel.ActualChapterIndex + 1;
            if (next < readerModel.Hq.Chapters.Count) {
                readerModel.NextChapter = readerModel.Hq.Chapters[next];
            }
            if (readerModel.ActualChapterIndex > 0) {
                readerModel.PreviousChapter = readerModel.Hq.Chapters[readerModel.ActualChapterIndex - 1];
            }

            _navigationManager.Navigate<T>("Reader", readerModel);
        }
    }
}
