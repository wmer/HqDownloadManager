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
    /// Interação lógica para MyLibraryPage.xam
    /// </summary>
    public partial class MyLibraryPage : Page {
        private MyLibraryController _controller;

        public MyLibraryPage(MyLibraryController controoller) {
            _controller = controoller;
            this.InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => (Resources["MyLibrary"] as MyLibraryViewModel).Columns = _controller.ChangeNumCollumns();

        private void HqLibraryGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => _controller.OpenDetails<DownloadDetailsPage>();
    }
}
