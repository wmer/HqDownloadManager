using HqDownloadManager.WPF.Models;
using HqDownloadManager.WPF.ViewModels;
using HqDownloadManager.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.Navigation;

namespace HqDownloadManager.WPF.Commands {
    public class PageNavigationCommand : CommandBase<NavigationViewModel> {
        private NavigationManager _navigationManager;

        public PageNavigationCommand(NavigationManager navigationManager) {
            _navigationManager = navigationManager;
        }

        public override bool CanExecute(NavigationViewModel parameter) =>
                                    parameter is NavigationViewModel navigationViewModel &&
                                    navigationViewModel.SelectedButton is MenuButton;

        public override void Execute(NavigationViewModel navigationView) {
            var menuButton = navigationView.SelectedButton;
            switch (menuButton.Label) {
                case "Menu":
                    navigationView.Opened = !navigationView.Opened;
                    break;
                case "Atualizações":
                    _navigationManager.Navigate<SourceUpdatesPage>("Updates");
                    break;
                case "Biblioteca":
                    _navigationManager.Navigate<SourceLibraryPage>("Biblioteca");
                    break;
                case "Meus Downloads":
                    _navigationManager.Navigate<MyLibraryPage>("Meus Downloads");
                    break;
                //case "Histórico de Leitura":
                //    _navigationManager.Navigate<ReaderHistoryPage>("Meu Histórico");
                //    break;
                //case "Minha Lista de Mangás":
                //    _navigationManager.Navigate<HqListPage>("Lista de Hqs");
                //    break;
                case "Gerenciador de Downloads":
                    _navigationManager.Navigate<DownloadPage>("Lista de Download");
                    break;
                    //case "Configurações":
                    //    _navigationManager.Navigate<ConfigurationPage>("Configurações");
                    //    break;
            }
        }
    }

    public class GoBackCommand : CommandBase {
        public override bool CanExecute(object parameter) =>
                                        NavigationManager.CanGoBack();
        public override void Execute(object parameter) =>
                                        NavigationManager.GoBack();
    }
}
