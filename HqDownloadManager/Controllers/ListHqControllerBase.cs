using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using DependencyInjectionResolver;
using HqDownloadManager.ViewModels.ComboBox;
using HqDownloadManager.ViewModels.List;
using HqDownloadManager.Core.Models;
using Windows.UI.Core;
using Windows.UI.Xaml;
using HqDownloadManager.Views;

namespace HqDownloadManager.Controllers {
    public class ListHqControllerBase : Controller {
        public ListHqControllerBase(DependencyInjection dependencyInjection) : base(dependencyInjection) {
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

        public async Task OpenHqDetails(bool isFinalized = false) {
            var Selectedhq = await GetSelectedHq(isFinalized);
            if (SourceManager.GetInfo(Selectedhq.Link) is Hq hq) {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    NavigationHelper.Navigate<HqDetailsPage>("Detalhes", hq);
                });
            }
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
                hq = SourceManager.GetInfo(selectedHq?.Link, isFinalized) as Hq;
            } else {
                hq = selectedHq;
            }


            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                Notification.Visibility = Visibility.Collapsed;
            });

            return hq;
        }
    }
}
