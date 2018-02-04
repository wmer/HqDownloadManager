using HqDownloadManager.Controller;
using HqDownloadManager.Controller.ViewModel.HqStatus;
using HqDownloadManager.Controller.ViewModel.MyDownloads;
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
    public sealed partial class DownloadDetailsPage : Windows.UI.Xaml.Controls.Page {
        private DownloadDetailsController _controller;
        public DownloadDetailsPage(DownloadDetailsController controller) {
            _controller = controller;
            this.InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }

        private void DownloadInfoChapters_ItemClick(object sender, ItemClickEventArgs e) => _controller.Read<HqReaderPage>(e.ClickedItem as Chapter);

        private void EditInfoBtn_Click(object sender, RoutedEventArgs e) => _controller.OpenEditor();

        private void DelHqBtn_Click(object sender, RoutedEventArgs e) => _controller.DeleteHq();

        private void SearchUpdatesBtn_Click(object sender, RoutedEventArgs e) => _controller.SearchUpdates();

        private void ChapterReading_ItemClick(object sender, ItemClickEventArgs e) {

        }

        private void SarchDetails_Click(object sender, RoutedEventArgs e) => _controller.SearchDetails();

        private void SaveDetails_Click(object sender, RoutedEventArgs e) => _controller.SaveDetails();

        private void BtnSaveStatus_Click(object sender, RoutedEventArgs e) => _controller.SaveEntry();

        private void BtnCloseStatus_Click(object sender, RoutedEventArgs e) =>
                                         (Resources["HqStatus"] as HqStatusViewModel).Visibility = Visibility.Collapsed;

        private void BtnAddSelected_Click(object sender, RoutedEventArgs e) => _controller.AddSelectedsToDownload();

        private void BtnCloseUpdates_Click(object sender, RoutedEventArgs e) =>
                                         (Resources["Details"] as DownloadDetailsViewModel).UpdateVisibility = Visibility.Collapsed;
    }
}
