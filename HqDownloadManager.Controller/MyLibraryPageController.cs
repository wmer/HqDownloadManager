using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Controller.ViewModel.MyDownloads;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqDownloadManager.Download.Models;
using HqManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controller {
    public class MyLibraryPageController : Windows.UI.Xaml.Controls.Page {
        protected ControlsHelper _controllerHelper;
        protected DownloadManager _downloadManager;
        private readonly NavigationHelper _navigationHelper;
        protected MyLibraryViewModel _libraryViewModel;
        protected NotificationViewModel _notification;

        public MyLibraryPageController(
                ControlsHelper controllerHelper,
                DownloadManager downloadManager,
                NavigationHelper navigationHelper) {
            _controllerHelper = controllerHelper;
            _downloadManager = downloadManager;
            _navigationHelper = navigationHelper;
            CoreEventHub.ProcessingProgress += CoreEventHub_ProcessingProgress;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _notification = _controllerHelper.FindResource<NotificationViewModel>("Notification");
            _libraryViewModel = _controllerHelper.FindResource<MyLibraryViewModel>("MyLibrary");
            _notification.Visibility = Visibility.Visible;

            Task<List<HqDownloadInfo>>.Factory.StartNew(() => {
                return _downloadManager.GetDownloadedHqsInfo();
            }).ContinueWith(async (list) => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _libraryViewModel.DownloadInfos = new ObservableCollection<HqDownloadInfo>(list.Result);
                    _notification.Visibility = Visibility.Collapsed;
                });
            });
        }

        public void OpenDetails<T>() where T : Windows.UI.Xaml.Controls.Page {
            Task.Run(async ()=> {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _notification.Visibility = Visibility.Visible;
                });
                var downloadInfo = _libraryViewModel.SelectedDownloadInfo;
                var info =_downloadManager.GetChaptersFromDownloadInfo(downloadInfo);

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _notification.Visibility = Visibility.Collapsed;
                    _navigationHelper.Navigate<T>("Detalhes", info);
                });
            });
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
