using HqDownloadManager.Core.Models;
using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.HqStatus;
using HqDownloadManager.Shared.ViewModel.MyDownloads;
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
    /// Interação lógica para DownloadDetailsPage.xam
    /// </summary>
    public partial class DownloadDetailsPage : System.Windows.Controls.Page {
        private DownloadDetailsController _controller;
        public DownloadDetailsPage(DownloadDetailsController controller) {
            _controller = controller;
            this.InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }

        private void DownloadInfoChapters_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => _controller.Read<HqReaderPage>(DownloadInfoChapters.SelectedItem as Chapter);

        private void EditInfoBtn_Click(object sender, RoutedEventArgs e) => _controller.OpenEditor();

        private void DelHqBtn_Click(object sender, RoutedEventArgs e) => _controller.DeleteHq();

        private void SearchUpdatesBtn_Click(object sender, RoutedEventArgs e) => _controller.SearchUpdates();

        private void ChapterReading_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => _controller.Read<HqReaderPage>(ChapterReading.SelectedItem as ReaderHistory);

        private void SarchDetails_Click(object sender, RoutedEventArgs e) => _controller.SearchDetails();

        private void SaveDetails_Click(object sender, RoutedEventArgs e) => _controller.SaveDetails();

        private void BtnSaveStatus_Click(object sender, RoutedEventArgs e) => _controller.SaveEntry();

        private void BtnCloseStatus_Click(object sender, RoutedEventArgs e) =>
                                         (Resources["HqStatus"] as HqStatusViewModel).Visibility = false;

        private void BtnAddSelected_Click(object sender, RoutedEventArgs e) => _controller.AddSelectedsToDownload();

        private void BtnCloseUpdates_Click(object sender, RoutedEventArgs e) =>
                                         (Resources["Details"] as DownloadDetailsViewModel).UpdateVisibility = false;

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => (Resources["Details"] as DownloadDetailsViewModel).Columns = _controller.ChangeNumCollumns();
    }
}
