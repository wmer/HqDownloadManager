using HqDownloadManager.Controller.ViewsController;
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

namespace HqDownloadManager.Views.DownloadPage {
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DownloadPage : DownloadPageBase {
        private bool _paused = false;

        public DownloadPage() {
            this.InitializeComponent();
        }

        private void BtnInitDownload_Click(object sender, RoutedEventArgs e) => Controller.Download();

        private void BtnPauseDownload_Click(object sender, RoutedEventArgs e) {
            _paused = !_paused;
            Controller.PauseResume(_paused);
        }

        private void Erase_Click(object sender, RoutedEventArgs e) => Controller.RemoveFromList();

        private void BtnEraseList_Click(object sender, RoutedEventArgs e) => Controller.Clearlist();

        private void BtnOrdenar_Click(object sender, RoutedEventArgs e) => Controller.OrderByName();
    }
}
