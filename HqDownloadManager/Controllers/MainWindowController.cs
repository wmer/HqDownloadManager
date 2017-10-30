using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Helpers;
using System.Windows.Controls;
using HqDownloadManager.ViewModels;
using System.Threading;
using DependencyInjectionResolver;
using HqDownloadManager.Compression;
using HqDownloadManager.Views;
using HqDownloadManager.Core;
using HqDownloadManager.Database;
using HqDownloadManager.Download;

namespace HqDownloadManager.Controllers {
    public class MainWindowController : Controller {
        private PaneViewModel _pane;

        public MainWindowController(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper, ZipManager zipManager) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper, zipManager) {
        }

        public override void Init(params object[] values) {
            base.Init();
            _pane = controlsHelper.FindResource<PaneViewModel>("Pane");
        }

        public void OpenCloseMenu() {
            var actualWidthOfMenu = _pane.Width;
            switch (actualWidthOfMenu) {
                case 34:
                    _pane.Width = 300;
                    break;
                case 300:
                    _pane.Width = 34;
                    break;
            }
        }

        public void Navigate<TObjectHandled>(object sender) where TObjectHandled : Control {
            var actualPage = controlsHelper.GetCurrentPage();
            var menuBtns = controlsHelper.FindResource<MenuButtonsViewModel>("MenuButtons");
            var pageTitle = controlsHelper.FindResource<PageTitleViewModel>("PageTitle");
            var clicked = (sender as TObjectHandled)?.Name;
            if (clicked == "hqUpdates" || clicked == "hqUpdatesLabel") {
                pageTitle.Title = "Atualizações";
                menuBtns.IsHqUpdates = true;
                menuBtns.IsAllHqs = false;
                menuBtns.IsMyLibrary = false;
                menuBtns.IsDownloadPage = false;
                menuBtns.IsSettings = false;
                menuBtns.IsReadingHistory = false;
                menuBtns.IsDownloadHistory = false;
                navigationHelper.Navigate<HqUpdatesPage>();
            }
            if (clicked == "allHqs" || clicked == "allHqsLabel") {
                pageTitle.Title = "Todos os Mangas";
                menuBtns.IsAllHqs = true;
                menuBtns.IsHqUpdates = false;
                menuBtns.IsMyLibrary = false;
                menuBtns.IsDownloadPage = false;
                menuBtns.IsSettings = false;
                menuBtns.IsReadingHistory = false;
                menuBtns.IsDownloadHistory = false;
                navigationHelper.Navigate<SourceLibraryPage>();
            }
            if (clicked == "myLibrary" || clicked == "myLibraryLabel") {
                pageTitle.Title = "Minha Biblioteca";
                menuBtns.IsMyLibrary = true;
                menuBtns.IsHqUpdates = false;
                menuBtns.IsAllHqs = false;
                menuBtns.IsDownloadPage = false;
                menuBtns.IsSettings = false;
                menuBtns.IsReadingHistory = false;
                menuBtns.IsDownloadHistory = false;
                navigationHelper.Navigate<MyLibraryPage>();
            }
            if (clicked == "readingHistory" || clicked == "readingHistoryLabel") {
                pageTitle.Title = "Histórico de Leitura";
                menuBtns.IsMyLibrary = false;
                menuBtns.IsHqUpdates = false;
                menuBtns.IsAllHqs = false;
                menuBtns.IsDownloadPage = false;
                menuBtns.IsSettings = false;
                menuBtns.IsReadingHistory = true;
                menuBtns.IsDownloadHistory = false;
               navigationHelper.Navigate<ReadingHistoryPage>();
            }
            if (clicked == "downloadHistory" || clicked == "downloadHistoryLabel") {
                pageTitle.Title = "Histórico de Download";
                menuBtns.IsMyLibrary = false;
                menuBtns.IsHqUpdates = false;
                menuBtns.IsAllHqs = false;
                menuBtns.IsDownloadPage = false;
                menuBtns.IsSettings = false;
                menuBtns.IsReadingHistory = false;
                menuBtns.IsDownloadHistory = true;
               navigationHelper.Navigate<DownloadHistoryPage>();
            }
            if (clicked == "downloadPage" || clicked == "downloadPageLabel") {
                pageTitle.Title = "Página de Download";
                menuBtns.IsDownloadPage = true;
                menuBtns.IsHqUpdates = false;
                menuBtns.IsAllHqs = false;
                menuBtns.IsMyLibrary = false;
                menuBtns.IsSettings = false;
                menuBtns.IsReadingHistory = false;
                menuBtns.IsDownloadHistory = false;
                navigationHelper.Navigate<DownloadPage>();
            }
            if (clicked == "settings" || clicked == "settingsLabel") {
                pageTitle.Title = "Configurações";
                menuBtns.IsSettings = true;
                menuBtns.IsHqUpdates = false;
                menuBtns.IsAllHqs = false;
                menuBtns.IsMyLibrary = false;
                menuBtns.IsDownloadPage = false;
                menuBtns.IsReadingHistory = false;
                menuBtns.IsDownloadHistory = false;
                navigationHelper.Navigate<ConfigurationPage>();
            }
        }
    }
}
