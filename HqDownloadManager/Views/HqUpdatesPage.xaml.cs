using DependencyInjectionResolver;
using HqDownloadManager.Controllers;
using HqDownloadManager.Helpers;
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

namespace HqDownloadManager.Views {
    /// <summary>
    /// Interação lógica para HqUpdatesPage.xam
    /// </summary>
    public partial class HqUpdatesPage : Page {
        private readonly DependencyInjection _dependency;
        private HqUpdatesController _hqUpdatesController;
        private NavigationHelper _navigationHelper;

        public HqUpdatesPage(DependencyInjection dependency) {
            _dependency = dependency;
            InitializeComponent();
            this.Loaded += HqUpdatesPage_Loaded;
        }

        private void HqUpdatesPage_Loaded(object sender, RoutedEventArgs e) {
            _hqUpdatesController = _dependency.Resolve<HqUpdatesController>();
            _navigationHelper = _dependency.Resolve<NavigationHelper>();
            _hqUpdatesController.Init();
            _hqUpdatesController.ShowHqUpdates();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (_hqUpdatesController != null) {
                _hqUpdatesController.ActualizeItemSizeAndCollumns();
            }
        }

        private void SourceHq_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) {

        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e) {

        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e) {

        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e) {

        }
    }
}
