using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.Helpers;
using HqDownloadManager.ViewModels;
using System.Windows;

namespace HqDownloadManager.Controllers {
    public class SourceLibraryController : Controller {
        private ObservableCollection<Hq> _hqList;
        private string nextPageLink;
        private string finalizedPage;

        public SourceLibraryController(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper) {
        }

        public override void Init(params object[] values) {
            _hqList = controlsHelper.FindResource<HqListViewModel>("HqList")?.Hqs;
        }

        public void ShowSourceLibrary() {
            var link = GetLinkForLibrary();
            if (!string.IsNullOrEmpty(link)) {
                ShowHqs(link);
            }
        }

        public void ShowNextPage() {
            if (!string.IsNullOrEmpty(nextPageLink)) {
                ShowHqs(nextPageLink, false);
            }
        }

        public void ShowOnlyFinalized() {
            if (!string.IsNullOrEmpty(finalizedPage)) {
                ShowHqs(finalizedPage);
            }
        }

        private void ShowHqs(string link, bool clearAfter = true) {
            Task<LibraryPage>.Factory.StartNew(() => {
                dispatcher.Invoke(() => {
                    if (clearAfter) {
                        _hqList.Clear();
                    }
                    notification.Visibility = Visibility.Visible;
                });
                return sourceManager.GetLibrary(link);
            }).ContinueWith((lib) => {
                foreach (var hq in lib.Result.Hqs) {
                    if (!string.IsNullOrEmpty(hq.Link)) {
                        dispatcher.Invoke(() => {
                            _hqList.Add(hq);
                        });
                    }
                }

                nextPageLink = lib.Result.NextPage;
                finalizedPage = lib.Result.FinalizedPage;
                dispatcher.Invoke(() => {
                    notification.Visibility = Visibility.Hidden;
                });
            });
        }
    }
}
