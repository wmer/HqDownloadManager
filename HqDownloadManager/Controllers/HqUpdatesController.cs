using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Helpers;
using System.Windows.Controls;
using HqDownloadManager.ViewModels;
using HqDownloadManager.Core;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using DependencyInjectionResolver;
using HqDownloadManager.Compression;
using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.Views;

namespace HqDownloadManager.Controllers {
    public class HqUpdatesController : ListHqControllerBase {
        private ObservableCollection<Hq> _hqList;

        public HqUpdatesController(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper, ZipManager zipManager) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper, zipManager) {
        }

        public override void Init(params object[] values) {
            base.Init();
            _hqList = controlsHelper.FindResource<HqListViewModel>("HqList")?.Hqs;
        }

        public void ShowHqUpdates() {
            Task<List<Hq>>.Factory.StartNew(() => {
                dispatcher.Invoke(() => {
                    _hqList.Clear();
                    notification.Visibility = Visibility.Visible;
                });
                var link = GetLinkForUpdates();
                var updates = new List<Hq>();
                if (!string.IsNullOrEmpty(link)) {
                    updates = sourceManager.GetUpdates(link);
                }
                return updates;
            }).ContinueWith((list) => {
                foreach (var hq in list.Result) {
                    if (!String.IsNullOrEmpty(hq.Link)) {
                        dispatcher.Invoke(() => {
                            _hqList.Add(hq);
                        });
                    }
                }
                dispatcher.Invoke(() => {
                    notification.Visibility = Visibility.Hidden;
                });
            });
        }
    }
}
