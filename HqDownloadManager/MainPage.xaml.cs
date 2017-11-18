using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Views;
using HqDownloadManager.Views.Configuration;
using HqDownloadManager.Views.DownloadPage;
using HqDownloadManager.Views.MyLibrary;
using HqDownloadManager.Views.Sourcelibrary;
using HqDownloadManager.Views.Updates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace HqDownloadManager {
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page {
        private readonly NavigationHelper _navigationHelper;
        public MainPage(Frame frame, NavigationHelper navigationHelper) {
            this.InitializeComponent();
            Navigation.Content = frame;
            _navigationHelper = navigationHelper;
        }

        private void Navigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
            if (args.IsSettingsInvoked) {
                _navigationHelper.Navigate<ConfigurationPage>("Configuração");
            } else {
                switch (args.InvokedItem) {
                    case "Atualizações":
                        _navigationHelper.Navigate<UpdatesPage>("Atualizações");
                        break;
                    case "Todos as Hqs":
                       _navigationHelper.Navigate<SourceLibraryPage>("Todos as Hqs");
                        break;
                    case "Meus Downloads":
                        _navigationHelper.Navigate<MyLibraryPage>("Meus Downloads");
                        break;
                    case "Meus Favoritos":
                        // _frame.Navigate(typeof(MusicPage));
                        break;

                    case "Gerenciador de Download":
                        _navigationHelper.Navigate<DownloadPage>("Downloads");
                        break;
                }
            }
        }
    }
}
