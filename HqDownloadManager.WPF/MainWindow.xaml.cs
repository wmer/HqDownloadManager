using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.MainWindow;
using HqDownloadManager.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Tools.Navigation;
using WPF.Tools.Navigation.Events;

namespace HqDownloadManager.WPF {
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window {
        private NavigationManager _navigationManager;

        public MainWindow(Frame rootFrame, NavigationManager navigationManager) {
            _navigationManager = navigationManager;
            InitializeComponent();
            Content.Children.Add(rootFrame);
            NavigationEventHub.Navigated += OnNavigated;
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            var list = sender as ListView;
            if (list.SelectedItem is MenuButton item) {
                switch (item.Label) {
                    case "Menu": {
                            if (Resources["NavigationView"] is NavigationViewModel openCloseBtn) {
                                openCloseBtn.Opened = !openCloseBtn.Opened;
                            }

                            break;
                        }
                    case "Atualizações":
                        _navigationManager.Navigate<SourceUpdatesPage>("Updates");
                        break;
                    case "Biblioteca":
                        _navigationManager.Navigate<SourceLibraryPage>("Biblioteca");
                        break;
                    case "Meus Downloads":
                        _navigationManager.Navigate<MyLibraryPage>("Meus Downloads");
                        break;
                    case "Histórico de Leitura":
                        _navigationManager.Navigate<ReaderHistoryPage>("Meus Downloads");
                        break;
                    case "Minha Lista de Mangás":
                        _navigationManager.Navigate<HqListPage>("Lista de Hqs");
                        break;
                    case "Gerenciador de Downloads":
                        _navigationManager.Navigate<DownloadPage>("Lista de Download");
                        break;
                    case "Configurações":
                        _navigationManager.Navigate<ConfigurationPage>("Configurações");
                        break;
                }
            }
        }

        private void OnNavigated(object sender, global::WPF.Tools.Navigation.Events.NavigationEventArgs e) {
            (Resources["NavigationView"] as NavigationViewModel).PageTitle = e.Title;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.Enter)) {
                FullScreen();
            }
            if (Keyboard.IsKeyDown(Key.Escape)) {
                NormalWindow();
            }
        }

        public void FullScreen() {
            var window = this;
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
            window.Left = 0;
            window.Top = 0;
            window.Width = SystemParameters.FullPrimaryScreenWidth;
            window.Height = SystemParameters.FullPrimaryScreenHeight;
            window.Topmost = true;
        }

        public void NormalWindow() {
            var window = this;
            window.WindowStyle = WindowStyle.ThreeDBorderWindow;
            window.ResizeMode = ResizeMode.CanResize;
            window.Left = 10;
            window.Top = 10;
            window.Width = 900;
            window.Height = 600;
            window.Topmost = false;
        }
    }
}
