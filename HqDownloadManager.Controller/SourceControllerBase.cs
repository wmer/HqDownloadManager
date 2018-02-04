using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Controller.ViewModel.ComboBox;
using HqDownloadManager.Controller.ViewModel.Details;
using HqDownloadManager.Controller.ViewModel.HqStatus;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Managers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqManager;
using HqManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controller {
    public class SourceControllerBase : Windows.UI.Xaml.Controls.Page {
        protected delegate IHqSourceManager LibraryOperation(out LibraryPage library, double timeCache = 4320);

        private SourceManager _sourceManager;
        protected ControlsHelper _controllerHelper;
        protected EntryManager _entryManager;
        protected DownloadManager _downloadManager;
        protected NotificationViewModel _notification;
        protected HqSourceLibrarySelectorViewModel _sourceSelector;
        protected DetailsViewModel _detailsViewModel;
        protected HqStatusViewModel _hqStatusView;
        protected Dictionary<string, IHqSourceManager> _sources;
        protected IHqSourceManager _actualSource;

        public SourceControllerBase(
            SourceManager sourceManager,
            EntryManager entryManage,
            DownloadManager downloadManager,
            ControlsHelper controllerHelper) {
            _sourceManager = sourceManager;
            _entryManager = entryManage;
            _downloadManager = downloadManager;
            _controllerHelper = controllerHelper;
            CoreEventHub.ProcessingProgress += CoreEventHub_ProcessingProgress;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _notification = _controllerHelper.FindResource<NotificationViewModel>("Notification");
            _sourceSelector = _controllerHelper.FindResource<HqSourceLibrarySelectorViewModel>("SourceSelector");
            _detailsViewModel = _controllerHelper.FindResource<DetailsViewModel>("DetailsView");
            _hqStatusView = _controllerHelper.FindResource<HqStatusViewModel>("HqStatus");
            _hqStatusView.PropertyChanged += HqStatusChanged;
            _sources = _sourceManager.GetSources();
            _actualSource = _sources[_sourceSelector.SelectedSource];
            _sourceSelector.PropertyChanged += SelectedSourceChanged;
        }


        public virtual void HqStatusChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedStatus") {

            }
        }

        public virtual void OpenDetails(Hq hq) {
            var splitView = _controllerHelper.Find<SplitView>("DetailsManga");
            splitView.IsPaneOpen = false;
            _hqStatusView.Visibility = Visibility.Collapsed;
            Task.Run(async () => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _notification.Visibility = Visibility.Visible;
                });
                var hqDetails = new Hq();
                _actualSource.GetInfo(hq.Link, out hqDetails);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _hqStatusView.Entry = _entryManager.GetHqEntry(hq);
                    _hqStatusView.Hq = hqDetails;
                    _detailsViewModel.Hq = hqDetails;
                    splitView.IsPaneOpen = true;
                    _notification.Visibility = Visibility.Collapsed;
                });
            });
        }

        public virtual void AddToDownloadList() {
            var hq = _hqStatusView.Hq;
            _entryManager.AddTo(ReadStatus.PRETENDO_LER, hq);
            _downloadManager.AddToDownloadList(hq, $"{CoreConfiguration.DownloadLocation}\\Hqs");
        }

        public virtual void AddSelectedsToDownload() {
            var listView = _controllerHelper.Find<ListView>("HqChapters");
            var hq = _hqStatusView.Hq;
            hq.Chapters = new List<Chapter>();
            foreach (var item in listView.SelectedItems) {
                hq.Chapters.Add(item as Chapter);
            }

            _entryManager.AddTo(ReadStatus.PRETENDO_LER, hq);
            _downloadManager.AddToDownloadList(hq, $"{CoreConfiguration.DownloadLocation}\\Hqs");
        }

        public virtual void SelectedSourceChanged(object sender, global::System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedSource") {
                _actualSource = _sources[_sourceSelector.SelectedSource];
            }
        }

        private void CoreEventHub_ProcessingProgress(object sender, ProcessingEventArgs ev) {
            Task.Run(async () => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    if (_notification != null) {
                        _notification.Message = ev.StateMessage;
                    }
                });
            });
        }
    }
}
