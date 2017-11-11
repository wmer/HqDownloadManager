using DependencyInjectionResolver;
using HqDownloadManager.Controllers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.ViewModels.Reader;
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
using System.Windows.Input;
using Windows.System;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace HqDownloadManager.Views {
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class HqReaderPage {
        private HqReaderController _controller;

        public HqReaderPage() {
            this.InitializeComponent();
            _controller = new HqReaderController(new DependencyInjection());
            Loaded += HqReaderPage_Loaded;
        }

        private void HqReaderPage_Loaded(object sender, RoutedEventArgs e) {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            _controller.InitReader(e.Parameter as Hq);
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == VirtualKey.Control) {
                _controller.ShowHideControllers();
            }
            if (e.Key == VirtualKey.Shift) {
                _controller.FullSCream();
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e) => _controller.PreviousChapter();

        private void Next_Click(object sender, RoutedEventArgs e) => _controller.NextChapter();

        private void itemFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e) => _controller.ActualizeactualPage();

    }
}
