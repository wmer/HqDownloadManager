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
using HqDownloadManager.Helpers;
using HqDownloadManager.Views;

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
                //_frame.Navigate(typeof(SettingsPage));
            } else {
                switch (args.InvokedItem) {
                    case "Atualizações":
                        _navigationHelper.Navigate<UpdatesPage>("Atualizações");
                        break;

                    case "Todos as Hqs":
                        _navigationHelper.Navigate<SourceLibraryPage>("Todos as Hqs");
                        break;

                    case "Meus Downloads":
                        // _frame.Navigate(typeof(GamesPage));
                        break;

                    case "Meus Favoritos":
                        // _frame.Navigate(typeof(MusicPage));
                        break;

                    case "Gerenciador de Download":
                        //_frame.Navigate(typeof(MyContentPage));
                        break;
                }
            }
        }
    }
}
