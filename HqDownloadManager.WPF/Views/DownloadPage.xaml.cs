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
    /// Interação lógica para DownloadPage.xam
    /// </summary>
    public partial class DownloadPage : Page {
        private DownloadController _controller;
        public DownloadPage(DownloadController controller) {
            _controller = controller;
            this.InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }

        private void BtnInitDownload_Click(object sender, RoutedEventArgs e) => _controller.Download();

        private void BtnPauseDownload_Click(object sender, RoutedEventArgs e) => _controller.PauseResumeDownload();

        private void BtnStopDownload_Click(object sender, RoutedEventArgs e) => _controller.StopDownload();

        private void Deleteitem_Click(object sender, RoutedEventArgs e) => _controller.ExcludeItem();
    }
}
