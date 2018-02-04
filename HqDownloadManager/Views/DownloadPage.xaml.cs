using HqDownloadManager.Controller;
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

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace HqDownloadManager.Views {
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DownloadPage : Page {
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
