using HqDownloadManager.Controller;
using HqDownloadManager.Controller.ViewModel.MyDownloads;
using HqDownloadManager.Controller.ViewModel.Reader;
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
    public sealed partial class HqReaderPage : Page {
        private HqReaderController _controller;

        public HqReaderPage(HqReaderController controller) {
            _controller = controller;
            this.InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }

        private void ScrollViewer_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {

        }

        private void FlipViewReader_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e) {

        }

        private void BtnCloseDetails_Click(object sender, RoutedEventArgs e) => DetailsManga.IsPaneOpen = false;

        private void ChapterReading_ItemClick(object sender, ItemClickEventArgs e) => (Resources["ReaderControl"] as ReaderViewModel).ActualChapterIndex = ChapterReading.SelectedIndex;
    }
}
