using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Download;
using HqDownloadManager.Download.Models;
using HqDownloadManager.Shared.ViewModel.MyDownloads;
using HqDownloadManager.Shared.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF.Tools.Navigation;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Controller {
    public class MyLibraryController : System.Windows.Controls.Page {
        protected DownloadManager _downloadManager;
        private readonly NavigationManager _navigationManager;
        protected MyLibraryViewModel _libraryViewModel;
        protected NotificationViewModel _notification;

        public MyLibraryController(
        DownloadManager downloadManager,
        NavigationManager navigationManager) {
            _downloadManager = downloadManager;
            _navigationManager = navigationManager;
            CoreEventHub.ProcessingProgress += CoreEventHub_ProcessingProgress;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _notification = ControlsHelper.FindResource<NotificationViewModel>("Notification");
            _libraryViewModel = ControlsHelper.FindResource<MyLibraryViewModel>("MyLibrary");
            _notification.Visibility = true;

            Task<List<HqDownloadInfo>>.Factory.StartNew(() => {
                return _downloadManager.GetDownloadedHqsInfo();
            }).ContinueWith((list) => {
                Dispatcher.Invoke(() => {
                    _libraryViewModel.DownloadInfos = new ObservableCollection<HqDownloadInfo>(list.Result);
                    _notification.Visibility = false;
                });
            });
        }

        public void OpenDetails<T>() where T : System.Windows.Controls.Page {
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    _notification.Visibility = true;
                });
                var downloadInfo = _libraryViewModel.SelectedDownloadInfo;
                if (downloadInfo != null) {
                    var info = _downloadManager.GetChaptersFromDownloadInfo(downloadInfo);

                    Dispatcher.Invoke(() => {
                        _notification.Visibility = false;
                        _navigationManager.Navigate<T>("Detalhes", info);
                    });
                }
            });
        }

        public int ChangeNumCollumns() {
            var grid = ControlsHelper.Find<Grid>("Principal");
            return Convert.ToInt32(grid.ActualWidth / 200);
        }

        private void CoreEventHub_ProcessingProgress(object sender, ProcessingEventArgs ev) {
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    if (_notification != null) {
                        _notification.Message = ev.StateMessage;
                    }
                });
            });
        }
    }
}
