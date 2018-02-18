using HqDownloadManager.Core.Models;
using HqDownloadManager.Shared.ViewModel.Details;
using HqDownloadManager.Shared.ViewModel.SourceUpdate;
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
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Views {
    public partial class SourceUpdatesPage : System.Windows.Controls.Page {
        private SourceUpdatesController _controller;
        public SourceUpdatesPage(SourceUpdatesController controller) {
            _controller = controller;
            InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }

        private void HqUpdatesGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (Resources["Updates"] is SourceUpdateViewModel sourceUpdates && 
                                sourceUpdates.SelectedUpdate is Update selectedUpdate) 
                _controller.OpenDetails(selectedUpdate.Hq);
        }

        private void Button_Click(object sender, RoutedEventArgs e) => (Resources["DetailsView"] as DetailsViewModel).Opened = false;

        private void AddAll_Click(object sender, RoutedEventArgs e) => _controller.AddToDownloadList();

        private void AddSelected_Click(object sender, RoutedEventArgs e) => _controller.AddSelectedsToDownload();

        private void BtnDownloadUpdates_Click(object sender, RoutedEventArgs e) => _controller.Downloadupdates();

        private void ReadUpdate_Click(object sender, RoutedEventArgs e) {
            if (sender is MenuItem menuItem) {
                if (menuItem.CommandParameter is ContextMenu parentContextMenu) {
                    var listViewItem = parentContextMenu.PlacementTarget.Find<ListViewItem>().Last();
                    var chapter = listViewItem.Content as Chapter;
                    _controller.ReadUpdate<HqReaderPage>(chapter);
                }
            }
        }

        private void ReadNow_Click(object sender, RoutedEventArgs e) {
            if (sender is MenuItem menuItem) {
                if (menuItem.CommandParameter is ContextMenu parentContextMenu) {
                    var listViewItem = parentContextMenu.PlacementTarget.Find<ListViewItem>().Last();
                    var chapter = listViewItem.Content as Chapter;
                    _controller.Read<HqReaderPage>(chapter);
                }
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => (Resources["Updates"] as SourceUpdateViewModel).Columns = _controller.ChangeNumCollumns();
    }
}
