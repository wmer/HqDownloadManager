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
using WinRTXamlToolkit.Controls.Extensions;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace HqDownloadManager.Views {
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class SourceLibraryPage : Windows.UI.Xaml.Controls.Page {
        private SourceLibraryController _controller;

        public SourceLibraryPage(SourceLibraryController controller) {
            _controller = controller;
            this.InitializeComponent();
            this.Loaded += SourceLibraryPage_Loaded;
        }

        private void SourceLibraryPage_Loaded(object sender, RoutedEventArgs e) {
            _controller.OnLoaded(sender, e);
            var scrollViewer = HqLibraryGrid.GetFirstDescendantOfType<ScrollViewer>();
            var scrollbars = scrollViewer.GetDescendantsOfType<ScrollBar>().ToList();
            var verticalBar = scrollbars.FirstOrDefault(x => x.Orientation == Orientation.Vertical);
            verticalBar.Scroll += VerticalBar_Scroll;
        }

        private void VerticalBar_Scroll(object sender, ScrollEventArgs e) => _controller.OnScroll(sender, e);        

        private void ReadNow_Click(object sender, RoutedEventArgs e) {

        }

        private void AddAll_Click(object sender, RoutedEventArgs e) => _controller.AddToDownloadList();

        private void AddSelected_Click(object sender, RoutedEventArgs e) => _controller.AddSelectedsToDownload();

        private void Button_Click(object sender, RoutedEventArgs e) => DetailsManga.IsPaneOpen = false;

        private void HqLibraryGrid_ItemClick(object sender, ItemClickEventArgs e) => _controller.OpenDetails(e.ClickedItem as Hq);

        private void BtnSaveStatus_Click(object sender, RoutedEventArgs e) => _controller.SaveEntry();

        private void BtnCloseStatus_Click(object sender, RoutedEventArgs e) => 
                                         (Resources["HqStatus"] as HqStatusViewModel).Visibility = Visibility.Collapsed;

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            var btn = (Button)sender;
            _controller.ShowLibraryWithLether(btn.Content as string);
        }

        private void BtnAll_Click(object sender, RoutedEventArgs e) => _controller.ShowLibrary();

        private void BtnFinalized_Click(object sender, RoutedEventArgs e) => _controller.ShowHqFinalized();
    }
}
