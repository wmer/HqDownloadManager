using HqDownloadManager.Core;
using HqDownloadManager.Helpers;
using HqDownloadManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using DependencyInjectionResolver;
using HqDownloadManager.Compression;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.Models;
using HqDownloadManager.Views;

namespace HqDownloadManager.Controllers {
    public abstract class Controller {
        protected ControlsHelper controlsHelper;
        protected NavigationHelper navigationHelper;
        protected ClickHelper clickHelper;
        protected SourceManager sourceManager;
        protected DependencyInjection dependencyInjection;
        protected UserLibraryContext userLibrary;
        protected DownloadManager downloadManager;
        protected NotificationHelper notificationHelper;
        protected ZipManager zipManager;

        protected readonly Dispatcher dispatcher;
        protected NotificationViewModel notification;
        protected UserPreferences userPreferences;
        protected static ObservableCollection<DownloadListItem> downloadList = new ObservableCollection<DownloadListItem>();

        protected Controller(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper,
            ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager,
            NotificationHelper notificationHelper, ZipManager zipManager) {
            this.dependencyInjection = dependencyInjection;
            this.controlsHelper = controlsHelper;
            this.navigationHelper = navigationHelper;
            this.clickHelper = clickHelper;
            this.userLibrary = userLibrary;
            this.downloadManager = downloadManager;
            this.notificationHelper = notificationHelper;
            this.zipManager = zipManager;
            this.sourceManager = sourceManager;
            this.sourceManager.ProcessingProgress += SourceManager_ProcessingProgress;

            dispatcher = Application.Current.MainWindow?.Dispatcher;
            notification = controlsHelper.FindResource<NotificationViewModel>("Notification");
        }

        public virtual void Init(params object[] values) {
            userPreferences = new UserPreferences();
            if (userLibrary.UserPreferences.FindOne(1) is UserPreferences userP) {
                userPreferences.Compress = userP.Compress;
                userPreferences.EraseFolder = userP.EraseFolder;
                userPreferences.DownloadPath = userP.DownloadPath;
                userPreferences.Shutdown = userP.Shutdown;
                userPreferences.Id = userP.Id;
            } else {
                userPreferences.Compress = false;
                userPreferences.EraseFolder = false;
                userPreferences.Shutdown = false;
                userPreferences.DownloadPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Downloads\\Mangas";
                userLibrary.UserPreferences.Save(userPreferences);
            }
        }

        public void AddToDownloadList(Hq hq) {
            var downloadItem = new DownloadListItem { Hq = hq, Status = "Não Baixado" };
            if (!downloadList.Contains(downloadItem))
            {
                dispatcher.Invoke(() => {
                    downloadList.Add(downloadItem);
                });
            }
        }

        public void FollowHq(Hq hq) {
            downloadManager.FollowHq(hq, userPreferences.DownloadPath);
        }

        public void Click(object sender, MouseButtonEventArgs e, Action action) => clickHelper.Click(sender, e, action);
        public void SetTimesOfClick(object sender, MouseButtonEventArgs e) => clickHelper.MouseDown(sender, e);

        protected virtual void SourceManager_ProcessingProgress(object sender, Core.CustomEventArgs.ProcessingEventArgs ev) {
            Task.Run(() => {
                if (notification != null) {
                    dispatcher.Invoke(() => {
                        notification.Message = ev.StateMessage;
                    });
                }
            });
        }
    }
}
