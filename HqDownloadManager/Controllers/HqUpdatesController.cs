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
using HqDownloadManager.Views;

namespace HqDownloadManager.Controllers {
    public class HqUpdatesController : Controller {
        private ObservableCollection<Hq> _hqList;

        public HqUpdatesController(ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager) :
            base(controlsHelper, navigationHelper, clickHelper, sourceManager) {
        }

        public override void Init() {
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
            })
            .ContinueWith((list) => {
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

        public void OpenHqDetails() {
            Task<Hq>.Factory.StartNew(GetSelectedHq)
                .ContinueWith((hqResult) => {
                if (hqResult.Result is Hq hq) {
                    dispatcher.Invoke(() => {
                        navigationHelper.Navigate<HqDetailsPage>(hq);
                    });
                }
            });
        }

        private Hq GetSelectedHq() {
            Hq hq = null;
            Hq selectedHq = null;
            var hqList = controlsHelper.Find<ListBox>("HqList");
            dispatcher.Invoke(() => {
                selectedHq = hqList.SelectedItem as Hq;
                notification.Visibility = Visibility.Visible;
            });

            hq = sourceManager.GetInfo(selectedHq?.Link) as Hq;

            dispatcher.Invoke(() => {
                notification.Visibility = Visibility.Hidden;
            });

            return hq;
        }

        public void ActualizeItemSizeAndCollumns() {
            var itemResource = controlsHelper.FindResource<ListBoxItemViewModel>("ListBoxItem");
            var page = controlsHelper.GetCurrentPage();
            var collumns = Convert.ToInt32(page.ActualWidth / 200);
            var width = (page.ActualWidth - 90) / collumns;
            var heigth = width + 75;

            if (itemResource == null) return;
            itemResource.Collums = collumns;
            itemResource.Width = width;
            itemResource.Height = heigth;
        }
    }
}
