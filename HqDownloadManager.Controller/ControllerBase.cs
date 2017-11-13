using DependencyInjectionResolver;
using HqDownloadManager.Controller.Databases;
using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Controller.Models;
using HqDownloadManager.Core;
using HqDownloadManager.Download;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HqDownloadManager.Controller {
    public class ControllerBase {
        protected DependencyInjection DependencyInjection;

        protected ControlsHelper ControlsHelper;
        protected NavigationHelper NavigationHelper;
        protected SourceManager SourceManager;
        protected UserLibraryContext UserLibrary;
        protected DownloadManager DownloadManager;
        //protected ZipManager ZipManager;
        //protected FollowManager FollowManager;
        //protected UpdateManager UpdateManager;

        protected CoreDispatcher Dispatcher;
        //protected NotificationViewModel Notification;
        //protected UserPreferencesViewModel UserPreferences;
        protected static ObservableCollection<DownloadListItem> DownloadList;
        protected static bool Downloading = false;
        private static string driverPath;
        protected static bool downloading = false;
    }
}
