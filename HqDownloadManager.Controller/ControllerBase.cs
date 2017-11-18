using DependencyInjectionResolver;
using HqDownloadManager.Compression;
using HqDownloadManager.Controller.Databases;
using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Controller.Models;
using HqDownloadManager.Controller.ViewModel.ConfigurationPage;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqDownloadManager.FollowUpdate;
using HqDownloadManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;

namespace HqDownloadManager.Controller {
    public class ControllerBase {
        protected DependencyInjection DependencyInjection;

        protected ControlsHelper ControlsHelper;
        protected NavigationHelper NavigationHelper;
        protected SourceManager SourceManager;
        protected UserLibraryContext UserLibrary;
        protected DownloadManager DownloadManager;
        protected FollowManager FollowManager;
        protected UpdateManager UpdateManager;
        protected ZipManager ZipManager;

        protected CoreDispatcher Dispatcher;
        protected NotificationViewModel Notification;
        protected UserPreferencesViewModel UserPreferences;
        protected static ObservableCollection<DownloadListItem> DownloadList;
        protected static bool Downloading = false;
        private static string driverPath;
        protected static bool downloading = false;

        public ControllerBase(DependencyInjection dependencyInjection) {
            if (string.IsNullOrEmpty(driverPath)) {
                driverPath = GetAssetFolder().Result;
            }
            CoreConfiguration.BaseDirectory = ApplicationData.Current.LocalFolder.Path;
            CoreConfiguration.WebDriversLocation = driverPath;
            CoreConfiguration.DownloadLocation = $"{ CoreConfiguration.BaseDirectory}\\Downloads";
            CoreConfiguration.DatabaseLocation = $"{ CoreConfiguration.BaseDirectory}\\Databases";
            DependencyInjection = dependencyInjection;
            this.ControlsHelper = DependencyInjection.Resolve<ControlsHelper>();
            this.NavigationHelper = DependencyInjection.Resolve<NavigationHelper>();
            this.UserLibrary = DependencyInjection.Resolve<UserLibraryContext>();
            this.SourceManager = DependencyInjection.Resolve<SourceManager>();
            this.DownloadManager = DependencyInjection.Resolve<DownloadManager>();
            this.ZipManager = DependencyInjection.Resolve<ZipManager>();
            this.FollowManager = DependencyInjection.Resolve<FollowManager>();
            this.UpdateManager = DependencyInjection.Resolve<UpdateManager>();

            this.Dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

            this.SourceManager.ProcessingProgress += SourceManager_ProcessingProgress;
            this.SourceManager.ProcessingError += SourceManagerOnProcessingError;
        }

        public virtual void Init(params object[] values) {
            Notification = ControlsHelper.FindResource<NotificationViewModel>("Notification");
            UserPreferences = new UserPreferencesViewModel();
            if (UserLibrary.UserPreferences.FindOne(1) is UserPreferences uP) {
                if (uP.UserPreferencesViewModel != null) {
                    var userP = uP.UserPreferencesViewModel.ToObject<UserPreferencesViewModel>();
                    UserPreferences.Compress = userP.Compress;
                    UserPreferences.EraseFolder = userP.EraseFolder;
                    UserPreferences.DownloadPath = $"{CoreConfiguration.DownloadLocation}\\Mangas"; ;
                    UserPreferences.Shutdown = userP.Shutdown;
                }
            } else {
                UserPreferences.Compress = false;
                UserPreferences.EraseFolder = false;
                UserPreferences.Shutdown = false;
                UserPreferences.DownloadPath = $"{CoreConfiguration.DownloadLocation}\\Mangas";
                var userP = new UserPreferences { UserPreferencesViewModel = UserPreferences.ToBytes() };
                UserLibrary.UserPreferences.Save(userP);
            }

            if (downloading) return;
            if (UserLibrary.DownloadList.FindOne(1) is DownloadList list) {
                DownloadList = list.List.ToObject<ObservableCollection<DownloadListItem>>();
            } else {
                DownloadList = new ObservableCollection<DownloadListItem>();
                var dw = new DownloadList { List = DownloadList.ToBytes() };
                UserLibrary.DownloadList.Save(dw);
            }
        }

        public void FollowHq(Hq hq) => FollowManager.FollowHq(hq);

        public async Task AddToDownloadList(Hq hq) {
            hq = SourceManager.GetInfo(hq.Link) as Hq; // Para retirar depois
            var downloadItem = new DownloadListItem { Hq = hq, Status = "Não Baixado" };
            if (!DownloadList.Contains(downloadItem)) {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    DownloadList.Add(downloadItem);
                    var dw = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
                    UserLibrary.DownloadList.Update(dw);
                });
            }
        }

        private async Task<string> GetAssetFolder() {
            var driverPath = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\WebDrivers");
            return driverPath.Path;
        }


        protected async virtual void SourceManager_ProcessingProgress(object sender, ProcessingEventArgs ev) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (Notification != null) {
                    Notification.Message = ev.StateMessage;
                }
            });
        }

        protected async virtual void SourceManagerOnProcessingError(object sender, ProcessingErrorEventArgs ev) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (Notification != null) {
                    Notification.Message = $"Um erro ocorreru : {ev.Exception}";
                }
            });
        }
    }
}
