using DependencyInjectionResolver;
using HqDownloadManager.Controller.Models;
using HqDownloadManager.Controller.ViewModel.DownloadPage;
using Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using HqDownloadManager.Core.Models;

namespace HqDownloadManager.Controller {
    public partial class DownloadController : ControllerBase {
        private DownloadListItem hqDownloading;
        private DownloadListViewModel downloadListViewModel;
        private ListView downloadListView;
        private int actualIndex;

        public DownloadController() : base() {
        }

        public override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            downloadListViewModel = ControlsHelper.FindResource<DownloadListViewModel>("HqList");
            downloadListView = ControlsHelper.Find<ListView>("DownloadHqs");
            downloadListViewModel.Hqs = DownloadList;
            HandlerEvents();
        }

        public void Download() {
            Task.Run(async () => {
                downloading = true;
                for (var i = 0; i < DownloadList.Count; i++) {
                    actualIndex = i;
                    hqDownloading = DownloadList[i];
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        downloadListView.SelectedItem = hqDownloading;
                        downloadListView.ScrollIntoView(downloadListView.SelectedItem);
                    });
                    if (hqDownloading.Status != "Baixado") {
                        try {
                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                                hqDownloading.Status = "Baixando...";
                            });
                            var dwL = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
                            UserLibrary.DownloadList.Update(dwL);
                            DownloadManager.Download(hqDownloading.Hq, UserPreferences.DownloadPath);
                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                                hqDownloading.Status = "Baixado";
                            });
                            dwL = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
                            UserLibrary.DownloadList.Update(dwL);
                        } catch (Exception e) {
                            ShowLog($"Um erro ocorreu durante o download de {hqDownloading.Hq.Title}. Causa: {e.Message}");
                        }
                    }
                }
                downloading = false;
            });
        }

        public void RemoveFromList() {
            var selected = downloadListView.SelectedItem as DownloadListItem;
            DownloadList.Remove(selected);
            var dwL = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
            UserLibrary.DownloadList.Update(dwL);
        }

        public void Clearlist() {
            DownloadList.Clear();
            var dwL = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
            UserLibrary.DownloadList.Update(dwL);
        }

        public void OrderByName() {
            var listOrder = DownloadList.OrderBy(c => c.Hq.Title).ToList();
            DownloadList = new ObservableCollection<DownloadListItem>(listOrder);
            var dwL = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
            UserLibrary.DownloadList.Update(dwL);
        }

        public void PauseResume(bool paused) => DownloadManager.PauseResumeDownload(paused);
    }
}
