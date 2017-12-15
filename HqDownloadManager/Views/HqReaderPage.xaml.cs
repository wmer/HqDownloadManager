using HqDownloadManager.Controller;
using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class HqReaderPage : HqReaderController {
        private Hq _hq;
        private ReaderViewModel _readerView;

        public HqReaderPage() {
            this.InitializeComponent();
        }

        public override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            InitControls();
            if (_hq != null) {
                InitReader(_hq);
            }
            if (_readerView != null) {
                OpenReader(_readerView);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter is Hq hq) {
                _hq = hq;
            }
            if (e.Parameter is ReaderViewModel readerView) {
                _readerView = readerView;
            }
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == VirtualKey.Control) {
                ShowHideControllers();
            }
            if (e.Key == VirtualKey.Shift) {
                FullSCream();
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e) => PreviousChapter();

        private void Next_Click(object sender, RoutedEventArgs e) => NextChapter();

        private void itemFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e) => ActualizeactualPage();

        private void ScrollViewer_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) => NormalizeZoom((ScrollViewer) sender, e);
    }
}
