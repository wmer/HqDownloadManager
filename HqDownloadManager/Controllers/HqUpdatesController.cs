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
            var listResult = Task<List<Hq>>.Factory.StartNew(() => {
                var link = GetLinkForUpdates();
                var updates = new List<Hq>();
                if (!String.IsNullOrEmpty(link)) {
                    updates = sourceManager.GetUpdates(link);
                }
                return updates;
            });
            listResult.ContinueWith((list) => {
                foreach (var hq in list.Result) {
                    if (!String.IsNullOrEmpty(hq.Link)) {
                        dispatcher.Invoke(() => {
                            _hqList.Add(hq);
                        });
                    }
                }
            });
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
