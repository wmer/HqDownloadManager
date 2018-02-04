using HqDownloadManager.Controller;
using HqDownloadManager.Controller.ViewModel.HqStatus;
using HqDownloadManager.Core.Models;
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
    public sealed partial class UpdatesPage : Windows.UI.Xaml.Controls.Page {
        private SourceUpdateController _controller;

        public UpdatesPage(SourceUpdateController controller) {
            _controller = controller;
            this.InitializeComponent();
            this.Loaded += UpdatesPage_Loaded;
        }

        private void UpdatesPage_Loaded(object sender, RoutedEventArgs e) => _controller.OnLoaded(sender, e);

        private void GridView_ItemClick(object sender, ItemClickEventArgs e) => _controller.OpenDetails((e.ClickedItem as Update).Hq);

        private void Button_Click(object sender, RoutedEventArgs e) => DetailsManga.IsPaneOpen = false;

        private void AddAll_Click(object sender, RoutedEventArgs e) => _controller.AddToDownloadList();

        private void AddSelected_Click(object sender, RoutedEventArgs e) => _controller.AddSelectedsToDownload();

        private void BtnDownloadUpdates_Click(object sender, RoutedEventArgs e) => _controller.Downloadupdates();

        private void ReadUpdate_Click(object sender, RoutedEventArgs e) {

        }

        private void ReadNow_Click(object sender, RoutedEventArgs e) {

        }
    }
}
