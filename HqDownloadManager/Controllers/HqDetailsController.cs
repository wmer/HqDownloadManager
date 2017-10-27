using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DependencyInjectionResolver;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.Helpers;
using HqDownloadManager.Views;

namespace HqDownloadManager.Controllers {
    public class HqDetailsController : Controller {

        public HqDetailsController(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper) {
        }

        public void OpenReader(ModelBase model) {
            var window = dependencyInjection
                .DefineDependency<HqReaderWindow>(0, model).Resolve<HqReaderWindow>(InstanceOptions.DiferentInstances);
            window.Top = 5;
            window.Left = 5;
            Application.Current.MainWindow = window;
            window.Show();
        }
    }
}
