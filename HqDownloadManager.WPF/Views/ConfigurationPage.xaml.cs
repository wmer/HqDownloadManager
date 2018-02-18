using HqDownloadManager.WPF.Controller;
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

namespace HqDownloadManager.WPF.Views {
    /// <summary>
    /// Interação lógica para ConfigurationPage.xam
    /// </summary>
    public partial class ConfigurationPage : Page {
        private ConfigurationController _controller;
        public ConfigurationPage(ConfigurationController controller) {
            _controller = controller;
            InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }

        private void BtnDbLocChange_Click(object sender, RoutedEventArgs e) => _controller.ChangeDbLocation();

        private void BtnCacheLocChange_Click(object sender, RoutedEventArgs e) => _controller.ChangeCacheLocation();

        private void BtnWDLocChange_Click(object sender, RoutedEventArgs e) => _controller.ChangeWebDriverLocation();

        private void BtnDownloadLocChange_Click(object sender, RoutedEventArgs e) => _controller.AddDownloadLocation();

        private void ApagarLoc_Click(object sender, RoutedEventArgs e) => _controller.DeleteDownloadLocation();
    }
}
