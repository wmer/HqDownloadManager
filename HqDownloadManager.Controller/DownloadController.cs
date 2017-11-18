using DependencyInjectionResolver;
using HqDownloadManager.Controller.Models;
using HqDownloadManager.Controller.ViewModel.DownloadPage;
using HqDownloadManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controller {
    public partial class DownloadController : ControllerBase {
        private DownloadListItem hqDownloading;
        private DownloadListViewModel downloadListViewModel;
        private ListView downloadListView;
        private int actualIndex;

        public DownloadController(DependencyInjection dependencyInjection) : base(dependencyInjection) {
        }

        public override void Init(params object[] values) {
            base.Init();
            downloadListViewModel = ControlsHelper.FindResource<DownloadListViewModel>("HqList");
            downloadListView = ControlsHelper.Find<ListView>("DownloadHqs");
            downloadListViewModel.Hqs = DownloadList;
            HandlerEvents();
        }

        public async Task Download() {
            await Task.Run(async () => {
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
                            hqDownloading.Status = "Baixando...";
                            var dw = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
                            UserLibrary.DownloadList.Update(dw);
                            await DownloadManager.Download(hqDownloading.Hq, UserPreferences.DownloadPath);
                            DownloadList[i].Status = "Baixado";
                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                                downloadListViewModel.Hqs[i] = hqDownloading;
                            });
                            dw = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
                            UserLibrary.DownloadList.Update(dw);
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
            var dw = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
            UserLibrary.DownloadList.Update(dw);
        }

        public void Clearlist() {
            DownloadList.Clear();
            var dw = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
            UserLibrary.DownloadList.Update(dw);
        }

        public void OrderByName() {
            var listOrder = DownloadList.OrderBy(c => c.Hq.Title).ToList();
            DownloadList = new ObservableCollection<DownloadListItem>(listOrder);
            var dw = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
            UserLibrary.DownloadList.Update(dw);
        }

        public void PauseResume(bool paused) => DownloadManager.PauseResumeDownload(paused);
    }
}
