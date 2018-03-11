using HqDownloadManager.Shared.ViewModel.List;
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
    /// Interação lógica para HqListPage.xam
    /// </summary>
    public partial class HqListPage : Page {
        private HqListController _controller;
        public HqListPage(HqListController controller) {
            _controller = controller;
            InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }

        private void BtnCloseDetails_Click(object sender, RoutedEventArgs e) => (Resources["MyList"] as HqListViewModel).ShowDetails = false;

        private void SaveBtn_Click(object sender, RoutedEventArgs e) => _controller.SaveEntry();

        private void DownloadAllBtn_Click(object sender, RoutedEventArgs e) => _controller.DownloadUpdates();

        private void DownloadSelectedBtn_Click(object sender, RoutedEventArgs e) => _controller.AddSelectedsToDownload();
    }
}
