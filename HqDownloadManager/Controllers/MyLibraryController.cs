using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Compression;
using HqDownloadManager.Core;
using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.Helpers;

namespace HqDownloadManager.Controllers {
    public class MyLibraryController : Controller {
        public MyLibraryController(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper, ZipManager zipManager) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper, zipManager) {
        }
    }
}
