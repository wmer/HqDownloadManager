using HqDownloadManager.Core.Models;
using HqDownloadManager.Shared.ViewModel.Details;
using HqDownloadManager.Shared.ViewModel.HqStatus;
using HqDownloadManager.Shared.ViewModel.SourceLibrary;
using HqDownloadManager.WPF.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Views {
    /// <summary>
    /// Interação lógica para SourceLibraryPage.xam
    /// </summary>
    public partial class SourceLibraryPage : System.Windows.Controls.Page {
        private SourceLibraryController _controller;

        public SourceLibraryPage(SourceLibraryController controller) {
            _controller = controller;
            this.InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }

        private void ReadNow_Click(object sender, RoutedEventArgs e) {
            if (sender is MenuItem menuItem) {
                if (menuItem.CommandParameter is ContextMenu parentContextMenu) {
                    var listViewItem = parentContextMenu.PlacementTarget.Find<ListViewItem>().FirstOrDefault();
                    var chapter = listViewItem.Content as Chapter;
                    _controller.Read<HqReaderPage>(chapter);
                }
            }
        }

        private void AddAll_Click(object sender, RoutedEventArgs e) => _controller.AddToDownloadList();

        private void AddSelected_Click(object sender, RoutedEventArgs e) => _controller.AddSelectedsToDownload();

        private void Button_Click(object sender, RoutedEventArgs e) => (Resources["DetailsView"] as DetailsViewModel).Opened = false;

        private void HqLibraryGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (Resources["Libraries"] is SourceLibraryViewModel source &&
                                source.SelectedHq is Hq selectedHq)
                _controller.OpenDetails(selectedHq);
        }

        private void BtnSaveStatus_Click(object sender, RoutedEventArgs e) => _controller.SaveEntry();

        private void BtnCloseStatus_Click(object sender, RoutedEventArgs e) =>
                                         (Resources["HqStatus"] as HqStatusViewModel).Visibility = false;

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            var btn = (Button)sender;
            _controller.ShowLibraryWithLether(btn.Content as string);
        }

        private void BtnAll_Click(object sender, RoutedEventArgs e) => _controller.ShowLibrary();

        private void BtnFinalized_Click(object sender, RoutedEventArgs e) => _controller.ShowHqFinalized();

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => (Resources["Libraries"] as SourceLibraryViewModel).Columns = _controller.ChangeNumCollumns();

        private void HqLibraryGrid_ScrollChanged(object sender, ScrollChangedEventArgs e) => _controller.OnScrollChanged(sender, e);
    }
}
