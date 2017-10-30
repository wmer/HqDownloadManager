using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DependencyInjectionResolver;
using HqDownloadManager.Compression;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.Helpers;
using HqDownloadManager.ViewModels;
using HqDownloadManager.Views;

namespace HqDownloadManager.Controllers {
    public abstract class ListHqControllerBase : Controller {

        protected ListHqControllerBase(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper, ZipManager zipManager) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper, zipManager) {
        }

        protected string GetLinkForUpdates() {
            var sourceSelected = GetSourceSelectedFromComboBox();
            var link = "";
            switch (sourceSelected) {
                case "MangaHost":
                    link = "https://mangashost.com/";
                    break;
                case "YesMangas":
                    link = "https://ymangas.com/";
                    break;
                case "UnionMangas":
                    link = "http://unionmangas.net";
                    break;
                case "MangasProject":
                    link = "https://mangas.zlx.com.br";
                    break;
            }
            return link;
        }

        protected string GetLinkForLibrary() {
            var sourceSelected = GetSourceSelectedFromComboBox();
            var link = "";
            switch (sourceSelected) {
                case "MangaHost":
                    link = "https://mangashost.com/mangas";
                    break;
                case "YesMangas":
                    link = "https://ymangas.com/mangas";
                    break;
                case "UnionMangas":
                    link = "http://unionmangas.net/mangas";
                    break;
                case "MangasProject":
                    link = "https://mangas.zlx.com.br/lista-de-mangas/ordenar-por-nome/todos";
                    break;
            }
            return link;
        }

        protected string GetSourceSelectedFromComboBox() {
            ComboBox _souceSelector = null;
            dispatcher.Invoke(() => {
                _souceSelector = controlsHelper.Find<ComboBox>("SourceHq");
            });

            ComboBoxItem itemSelected = null;
            String itemSelectedontent = null;
            dispatcher.Invoke(() => {
                itemSelected = _souceSelector.SelectedItem as ComboBoxItem;
                itemSelectedontent = itemSelected.Content as string;
            });
            return itemSelectedontent;
        }

        public void OpenHqDetails(bool isFinalized = false) {
            Task<Hq>.Factory.StartNew(() => GetSelectedHq(isFinalized))
                .ContinueWith((hqResult) => {
                    if (hqResult.Result is Hq hq) {
                        dispatcher.Invoke(() => {
                            navigationHelper.Navigate<HqDetailsPage>(hq);
                        });
                    }
                });
        }

        public void AddInDownloadList(bool isFinalized = false) {
            Task<Hq>.Factory.StartNew(() => GetSelectedHq(isFinalized))
                .ContinueWith((hqResult) => {
                    if (hqResult.Result is Hq hq) {
                        AddToDownloadList(hq);
                    }
                });
        }

        protected Hq GetSelectedHq(bool isFinalized = false) {
            Hq hq = null;
            Hq selectedHq = null;
            ListBox hqList = null;
            dispatcher.Invoke(() => {
                hqList = controlsHelper.Find<ListBox>("HqList");
            });

            dispatcher.Invoke(() => {
                selectedHq = hqList.SelectedItem as Hq;
                notification.Visibility = Visibility.Visible;
            });

            hq = sourceManager.GetInfo(selectedHq?.Link, isFinalized) as Hq;

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
