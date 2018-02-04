using HqDownloadManager.Controller.CustomEvents;
using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Controller.ViewModel.MainPage;
using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Core;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Download;
using HqManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace HqDownloadManager.Controller {
    public class HqReaderController : Windows.UI.Xaml.Controls.Page {
        protected ControlsHelper _controllerHelper;
        protected DownloadManager _downloadManager;
        private SourceManager _sourceManager;
        protected EntryManager _entryManager;
        protected NotificationViewModel _notification;
        private ReaderViewModel _readerViewModel;
        private NavigationViewModel _pageTitleView;
        private FlipView _flipView;

        public HqReaderController(
                ControlsHelper controllerHelper,
                 DownloadManager downloadManager,
                 SourceManager sourceManager,
                 EntryManager entryManager) {
            _controllerHelper = controllerHelper;
            _downloadManager = downloadManager;
            _sourceManager = sourceManager;
            _entryManager = entryManager;
            NavigationEventHub.Navigated += NavigationEventHub_Navigated;
            CoreEventHub.ProcessingProgress += CoreEventHub_ProcessingProgress;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _notification = _controllerHelper.FindResource<NotificationViewModel>("Notification");
            _pageTitleView = _controllerHelper.FindResource<NavigationViewModel>("TitleViewModel");
            _flipView = _controllerHelper.Find<FlipView>("FlipViewReader");
        }


        private void NavigationEventHub_Navigated(object sender, NavigationEventArgs e) {
            _readerViewModel = _controllerHelper.FindResource<ReaderViewModel>("ReaderControl");
            if (e.ExtraContent != null && _readerViewModel != null) {
                var reader = e.ExtraContent as ReaderViewModel;
                _readerViewModel.Hq = reader.Hq;
                _readerViewModel.ActualChapter = reader.ActualChapter;
                _readerViewModel.ActualChapterIndex = reader.ActualChapterIndex;
                _readerViewModel.PreviousChapter = reader.PreviousChapter;
                _readerViewModel.NextChapter = reader.NextChapter;
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
