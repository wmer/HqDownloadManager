using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Helpers;
using System.Windows.Controls;
using HqDownloadManager.ViewModels;
using System.Threading;
using HqDownloadManager.Views;
using HqDownloadManager.Core;

namespace HqDownloadManager.Controllers {
    public class MainWindowController : Controller {
        private PaneViewModel _pane;

        public MainWindowController(ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager) : 
            base(controlsHelper, navigationHelper, clickHelper, sourceManager) {
        }

        public override void Init() {
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
                pageTitle.PageTitle = "Atualizações";
                menuBtns.IsHqUpdates = true;
                menuBtns.IsAllHqs = false;
                menuBtns.IsMyLibrary = false;
                menuBtns.IsDownloadPage = false;
                menuBtns.IsSettings = false;
                navigationHelper.Navigate<HqUpdatesPage>();
            }
            if (clicked == "allHqs" || clicked == "allHqsLabel") {
                pageTitle.PageTitle = "Todos os Mangas";
                menuBtns.IsAllHqs = true;
                menuBtns.IsHqUpdates = false;
                menuBtns.IsMyLibrary = false;
                menuBtns.IsDownloadPage = false;
                menuBtns.IsSettings = false;
            }
            if (clicked == "myLibrary" || clicked == "myLibraryLabel") {
                pageTitle.PageTitle = "Meus Downloads";
                menuBtns.IsMyLibrary = true;
                menuBtns.IsHqUpdates = false;
                menuBtns.IsAllHqs = false;
                menuBtns.IsDownloadPage = false;
                menuBtns.IsSettings = false;
            }
            if (clicked == "downloadPage" || clicked == "downloadPageLabel") {
                pageTitle.PageTitle = "Página de Download";
                menuBtns.IsDownloadPage = true;
                menuBtns.IsHqUpdates = false;
                menuBtns.IsAllHqs = false;
                menuBtns.IsMyLibrary = false;
                menuBtns.IsSettings = false;
            }
            if (clicked == "settings" || clicked == "settingsLabel") {
                pageTitle.PageTitle = "Configurações";
                menuBtns.IsSettings = true;
                menuBtns.IsHqUpdates = false;
                menuBtns.IsAllHqs = false;
                menuBtns.IsMyLibrary = false;
                menuBtns.IsDownloadPage = false;
            }
        }
    }
}
