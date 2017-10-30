using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DependencyInjectionResolver;
using HqDownloadManager.Compression;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.Helpers;
using HqDownloadManager.Views;

namespace HqDownloadManager.Controllers {
    public class HqDetailsController : Controller {

        public HqDetailsController(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper, ZipManager zipManager) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper, zipManager) {
        }

        public void OpenReader(Hq model) {
            var window = dependencyInjection
                .DefineDependency<HqReaderWindow>(0, model).Resolve<HqReaderWindow>(InstanceOptions.DiferentInstances);
            window.Top = 5;
            window.Left = 5;
            Application.Current.MainWindow = window;
            window.Show();
        }

        public void ReadNow(Hq hq) {
            var tempHq = hq;
            tempHq.Chapters = GetSelectedChapters();
            OpenReader(tempHq);
        }

        public void AddChaptersSelectedToDownload(Hq hq) {
            var tempHq = hq;
            tempHq.Chapters = GetSelectedChapters();
            AddToDownloadList(tempHq);
        }

        private List<Chapter> GetSelectedChapters() {
            var listChapters = new List<Chapter>();
            ListView list = null;
            dispatcher.Invoke(() => {
                list = controlsHelper.Find<ListView>("HqChapters");
                var seleteds = list.SelectedItems;
                foreach (var item in seleteds) {
                    listChapters.Add(item as Chapter);
                }
            });
            return listChapters;
        }
    }
}
