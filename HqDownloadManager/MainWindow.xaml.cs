using HqDownloadManager.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DependencyInjectionResolver;
using HqDownloadManager.ViewModels;
using HqDownloadManager.Controllers;

namespace HqDownloadManager {
    public partial class MainWindow : Window {
        private Frame _frame;
        private MainWindowController _mainWindowController;
        private readonly DependencyInjection _dependency;

        public MainWindow(Frame frame, DependencyInjection dependency) {
            InitializeComponent();
            this._frame = frame;
            this._dependency = dependency;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            Content.Children.Add(_frame);
            _mainWindowController = _dependency.Resolve<MainWindowController>();
            _mainWindowController.Init();
            var pageTitle = Resources["PageTitle"] as PageTitleViewModel;
            pageTitle.Title = "Atualizações";
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if ((sender as Button)?.Name == "OCMenu") {
                _mainWindowController.OpenCloseMenu();
            }
            _mainWindowController.Navigate<ToggleButton>(sender);
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) => _mainWindowController.SetTimesOfClick(sender, e);
        private void Label_MouseUp(object sender, MouseButtonEventArgs e) => _mainWindowController.Click(sender, e, () => {
            if ((sender as Button)?.Name == "OCMenu") {
                _mainWindowController.OpenCloseMenu();
            }
            _mainWindowController.Navigate<Label>(sender);
        });
    }
}
