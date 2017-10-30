using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Compression;
using HqDownloadManager.Core;
using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.Download.CustomEventArgs;
using HqDownloadManager.Helpers;
using HqDownloadManager.Models;
using HqDownloadManager.ViewModels;

namespace HqDownloadManager.Controllers {
    public partial class DownloadController : Controller {
        private DownloadListViewModel downloadListViewModel;
        private LogViewModel logViewModel;
        private HqDownloadingViewModel hqDownloading;

        public DownloadController(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper, ZipManager zipManager) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper, zipManager) {
        }

        public override void Init(params object[] values) {
            base.Init();
            userPreferences = userLibrary.UserPreferences.FindOne(1);
            logViewModel = controlsHelper.FindResource<LogViewModel>("Log");
            hqDownloading = controlsHelper.FindResource<HqDownloadingViewModel>("DownloadingHq");
            downloadListViewModel = controlsHelper.FindResource<DownloadListViewModel>("HqList");
            downloadListViewModel.Hqs = downloadList;
            HandlerEvents();
        }

        public void Download() {
            Task.Run(() => {
                foreach (var hq in downloadListViewModel.Hqs) {
                    try {
                        hq.Status = "Baixando...";
                        downloadListViewModel.OnPropertyChanged("Hqs");
                        downloadManager.Download(hq.Hq, userPreferences.DownloadPath);
                        hq.Status = "Baixado";
                        downloadListViewModel.OnPropertyChanged("Hqs");
                    } catch (Exception e) {
                        ShowLog($"Um erro ocorreu durante o download de {hq.Hq.Title}. Causa: {e.Message}");
                    }
                }
                dispatcher.Invoke(() => {
                    downloadList.Clear();
                    downloadListViewModel.Hqs.Clear();
                });
            });
        }

        public void PauseResume(bool paused) => downloadManager.PauseResumeDownload(paused);
    }
}
