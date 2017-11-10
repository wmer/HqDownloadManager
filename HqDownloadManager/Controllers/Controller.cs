using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using DependencyInjectionResolver;
using HqDownloadManager.Compression;
using HqDownloadManager.Core;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Databases;
using HqDownloadManager.Download;
using HqDownloadManager.Follow;
using HqDownloadManager.Helpers;
using HqDownloadManager.Models;
using HqDownloadManager.ViewModels.SettingsPage;
using HqDownloadManager.Utils;
using HqDownloadManager.ViewModels.List;
using HqDownloadManager.Core.Models;

namespace HqDownloadManager.Controllers {
    public class Controller {
        protected DependencyInjection DependencyInjection;

        protected ControlsHelper ControlsHelper;
        protected NavigationHelper NavigationHelper;
        protected SourceManager SourceManager;
        protected UserLibraryContext UserLibrary;
        protected DownloadManager DownloadManager;
        protected ZipManager ZipManager;
        protected FollowManager FollowManager;
        protected UpdateManager UpdateManager;

        protected CoreDispatcher Dispatcher;
        protected NotificationViewModel Notification;
        protected UserPreferencesViewModel UserPreferences;
        //protected static ObservableCollection<DownloadListItem> DownloadList;
        protected static bool Downloading = false;
        private static string driverPath;

        public Controller(DependencyInjection dependencyInjection)
        {
            if (string.IsNullOrEmpty(driverPath)) {
                driverPath = GetAssetFolder().Result;
            }
            var dbDirectory = ApplicationData.Current.LocalFolder.Path;
            DependencyInjection = dependencyInjection;
            this.ControlsHelper = DependencyInjection.Resolve<ControlsHelper>();
            this.NavigationHelper = DependencyInjection.Resolve<NavigationHelper>();
            this.UserLibrary = DependencyInjection.Resolve<UserLibraryContext>();
            this.SourceManager = DependencyInjection
                                        .DefineDependency<SourceManager>(0, dbDirectory)
                                        .DefineDependency<SourceManager>(1, driverPath)
                                        .Resolve<SourceManager>();
            this.DownloadManager = DependencyInjection
                                        .DefineDependency<DownloadManager>(0, dbDirectory)
                                        .Resolve<DownloadManager>();
            this.ZipManager = DependencyInjection.Resolve<ZipManager>();
            this.FollowManager = DependencyInjection
                                        .DefineDependency<FollowManager>(0, dbDirectory)
                                        .Resolve<FollowManager>();
            this.UpdateManager = DependencyInjection
                                        .DefineDependency<UpdateManager>(0, dbDirectory)
                                        .Resolve<UpdateManager>();

            this.Dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

            this.SourceManager.ProcessingProgress += SourceManager_ProcessingProgress;
            this.SourceManager.ProcessingError += SourceManagerOnProcessingError;
        }

        public virtual void Init(params object[] values) {
            Notification = ControlsHelper.FindResource<NotificationViewModel>("Notification");
            UserPreferences = new UserPreferencesViewModel();
            if (UserLibrary.UserPreferences.FindOne(1) is UserPreferences uP) {
                var userP = uP.UserPreferencesViewModel.ToObject<UserPreferencesViewModel>();
                UserPreferences.Compress = userP.Compress;
                UserPreferences.EraseFolder = userP.EraseFolder;
                UserPreferences.DownloadPath = userP.DownloadPath;
                UserPreferences.Shutdown = userP.Shutdown;
            } else {
                UserPreferences.Compress = false;
                UserPreferences.EraseFolder = false;
                UserPreferences.Shutdown = false;
                UserPreferences.DownloadPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Downloads\\Mangas";
                var userP = new UserPreferences { UserPreferencesViewModel = UserPreferences.ToBytes() };
                UserLibrary.UserPreferences.Save(userP);
            }
        }

        public void FollowHq(Hq hq) => FollowManager.FollowHq(hq);

        private async Task<string> GetAssetFolder() {
            var driverPath = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\WebDrivers");
            return driverPath.Path;
        }


        protected virtual void SourceManager_ProcessingProgress(object sender, ProcessingEventArgs ev) {
            Task.Run(() => {
                if (Notification != null) {
                    Notification.Message = ev.StateMessage;
                }
            });
        }

        protected virtual void SourceManagerOnProcessingError(object sender, ProcessingErrorEventArgs ev) {
            Task.Run(() => {
                if (Notification != null) {
                    Notification.Message = $"Um erro ocorreru : {ev.Exception}";
                }
            });
        }
    }
}
  