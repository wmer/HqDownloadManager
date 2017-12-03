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
    public sealed partial class DownloadPage : DownloadController {
        private bool _paused = false;

        public DownloadPage() {
            this.InitializeComponent();
        }

        private void BtnInitDownload_Click(object sender, RoutedEventArgs e) => Download();

        private void BtnPauseDownload_Click(object sender, RoutedEventArgs e) {
            _paused = !_paused;
            PauseResume(_paused);
        }

        private void Erase_Click(object sender, RoutedEventArgs e) => RemoveFromList();

        private void BtnEraseList_Click(object sender, RoutedEventArgs e) => Clearlist();

        private void BtnOrdenar_Click(object sender, RoutedEventArgs e) => OrderByName();
    }
}
