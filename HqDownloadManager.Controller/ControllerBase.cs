using DependencyInjectionResolver;
using HqDownloadManager.Controller.Databases;
using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Controller.Models;
using HqDownloadManager.Controller.ViewModel.ConfigurationPage;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Models;
using System;
using Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HqDownloadManager.Download;
using HqDownloadManager.FollowUpdate;
using HqDownloadManager.Download.Databases;
using HqDownloadManager.Download.Models;

namespace HqDownloadManager.Controller {
    public class ControllerBase : Windows.UI.Xaml.Controls.Page {
        protected DependencyInjection DependencyInjection;

        protected ControlsHelper ControlsHelper;
        protected NavigationHelper NavigationHelper;
        protected SourceManager SourceManager;
        protected UserLibraryContext UserLibrary;
        protected DownloadManager DownloadManager;
        protected FollowManager FollowManager;
        protected UpdateManager UpdateManager;
        //protected ZipManager ZipManager;
        
        protected NotificationViewModel Notification;
        protected UserPreferencesViewModel UserPreferences;
        protected static ObservableCollection<DownloadListItem> DownloadList;
        protected static bool Downloading = false;
        private static string driverPath;
        protected static bool downloading = false;

        public ControllerBase() : this(new DependencyInjection()) {

        }

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
            //this.ZipManager = DependencyInjection.Resolve<ZipManager>();
            this.FollowManager = DependencyInjection.Resolve<FollowManager>();
            this.UpdateManager = DependencyInjection.Resolve<UpdateManager>();
            

            this.SourceManager.ProcessingProgress += SourceManager_ProcessingProgress;
            this.SourceManager.ProcessingError += SourceManagerOnProcessingError;
            Loaded += OnLoaded;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
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
            if (UserLibrary.DownloadList.FindOne(1) is DownloadList dwList) {
                DownloadList = dwList.List.ToObject<ObservableCollection<DownloadListItem>>();
            } else {
                DownloadList = new ObservableCollection<DownloadListItem>();
                var dwL = new DownloadList { List = DownloadList.ToBytes() };
                UserLibrary.DownloadList.Save(dwL);
            }

            //Aliment();
        }

        private async void Aliment() {
            //var downloads = await DownloadManager.GetDownloadedHqsInfo();
            //foreach (var hqDownloaded in downloads) {
            //    var hq = hqDownloaded.HqDownloaded.ToObject<Hq>();
            //    if (hq != null) {
            //        await AddToDownloadList(hq);
            //    }
            //}
            
        }


        public void FollowHq(Hq hq) => FollowManager.FollowHq(hq);

        public async Task AddToDownloadList(Hq hq) {
            hq = SourceManager.GetInfo<Hq>(hq.Link, false, -1);
            if (hq != null) {
                var downloadItem = new DownloadListItem { Hq = hq };
                try {
                    if (!DownloadList.Contains(downloadItem)) {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                            DownloadList.Add(downloadItem);
                            var dwL = new DownloadList { Id = 1, List = DownloadList.ToBytes() };
                            UserLibrary.DownloadList.Update(dwL);

                        });
                    }
                }catch { }
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
