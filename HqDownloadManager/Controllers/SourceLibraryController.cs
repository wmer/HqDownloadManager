using DependencyInjectionResolver;
using HqDownloadManager.Core.Models;
using HqDownloadManager.ViewModels.List;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controllers {
    public class SourceLibraryController : ListHqControllerBase {
        private ObservableCollection<Hq> _hqList;
        private HqListViewModel _hqListView;
        private string nextPageLink;
        private string finalizedPage;

        private object _lock1 = new object();

        public SourceLibraryController(DependencyInjection dependencyInjection) : base(dependencyInjection) {
        }

        public override void Init(params object[] values) {
            base.Init();
            _hqListView = ControlsHelper.FindResource<HqListViewModel>("HqList");
            _hqList = _hqListView?.Hqs;
        }

        public void ShowSourceLibrary() {
            var link = GetLinkForLibrary();
            if (!string.IsNullOrEmpty(link)) {
                ShowHqs(link);
            }
        }

        public void ShowOnlyFinalized() {
            if (!string.IsNullOrEmpty(finalizedPage)) {
                ShowHqs(finalizedPage);
            }
        }

        public async Task ShowLether(string key) {
            GridView souceSelector = null;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                souceSelector = ControlsHelper.Find<GridView>("LetterList");
                var dic = souceSelector.ItemsSource as Dictionary<string, string>;
                if (!string.IsNullOrEmpty(dic[key])) {
                    ShowHqs(dic[key]);
                }
            });
        }

        public void ShowNextPage() {
            if (!string.IsNullOrEmpty(nextPageLink)) {
                ShowHqs(nextPageLink, false);
            }
        }

        private async Task ShowHqs(string link, bool clearAfter = true) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (clearAfter) {
                    _hqList.Clear();
                }
                Notification.Visibility = Visibility.Visible;
            });

            var lib = await Task<LibraryPage>.Factory.StartNew(() => {
               return  SourceManager.GetLibrary(link);
            });

            foreach (var hq in lib.Hqs) {
                if (!string.IsNullOrEmpty(hq.Link)) {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        _hqList.Add(hq);
                    });
                }
            }

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                _hqListView.Lethers = lib.Letras;
                nextPageLink = lib.NextPage;
                finalizedPage = lib.FinalizedPage;
                Notification.Visibility = Visibility.Collapsed;
            });
        }
    }
}
