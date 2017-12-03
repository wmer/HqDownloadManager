using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Controller.ViewModel.ComboBox;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Core.Models;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace HqDownloadManager.Controller {
    public class ListHqControllerBase : ControllerBase {
        public ListHqControllerBase() : base() {
        }

        public async Task AddToDownloadList() {
            var Selectedhq = await GetSelectedHq(false);
            await AddToDownloadList(Selectedhq);
        }

        public async Task FollowHq() {
            var Selectedhq = await GetSelectedHq(false);
            //FollowHq(Selectedhq);
        }

        protected string GetLinkForUpdates() => GetSelectedSourceLink<UpdateSource>("Sources");

        protected string GetLinkForLibrary() => GetSelectedSourceLink<LibrarySource>("Sources");

        protected string GetSelectedSourceLink<T>(string key) where T : ComboBoxViewModelBase {
            var sourceSelected = ControlsHelper.FindResource<T>(key);
            var link = "";
            if (sourceSelected != null) {
                link = sourceSelected.SelectedSourceLink?.Link;
            }
            return link;
        }

        public async Task OpenHqDetails<T>(bool isFinalized = false) {
            var Selectedhq = await GetSelectedHq(isFinalized);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                NavigationHelper.Navigate<T>("Detalhes", Selectedhq);
            });
        }

        public void ActualizeItemSizeAndCollumns() {
            var itemResource = ControlsHelper.FindResource<HqListViewModel>("HqList");
            var page = ControlsHelper.GetCurrentPage();
            var collumns = page.ActualWidth / 160.45;
            var width = (page.ActualWidth) / collumns;
            var heigth = width + 70;

            if (itemResource == null) return;
            itemResource.Width = width;
            itemResource.Height = heigth;
        }

        protected async Task<Hq> GetSelectedHq(bool isFinalized = false) {
            Hq hq = null;
            Hq selectedHq = null;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                Notification.Visibility = Visibility.Visible;
                var hqList = ControlsHelper.FindResource<HqListViewModel>("HqList");
                selectedHq = hqList.SelectedItem;
            });

            if (selectedHq?.Chapters == null || selectedHq?.Chapters.Count == 0) {
                selectedHq = SourceManager.GetInfo<Hq>(selectedHq?.Link, isFinalized);
            }

            hq = selectedHq;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                Notification.Visibility = Visibility.Collapsed;
            });

            return hq;
        }
    }
}
