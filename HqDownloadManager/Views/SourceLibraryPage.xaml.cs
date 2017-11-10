using DependencyInjectionResolver;
using HqDownloadManager.Controllers;
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
    public sealed partial class SourceLibraryPage : Page {
        private readonly SourceLibraryController _controller;
        private bool _isFinalized = false;

        public SourceLibraryPage() {
            this.InitializeComponent();
            _controller = new DependencyInjection().Resolve<SourceLibraryController>();
            Loaded += SourceLibraryPage_Loaded; ;
        }

        private void SourceLibraryPage_Loaded(object sender, RoutedEventArgs e) {
            _controller.Init();
            var scrollViewr = HqlistGrid.GetFirstDescendantOfType<ScrollViewer>();
            scrollViewr.ViewChanged += ScrollViewr_ViewChanged;

            if ((bool)CheckboxOnlyFinalized.IsChecked) {
                _controller?.ShowOnlyFinalized();
            } else {
                _controller.ShowSourceLibrary();
            }
        }

        private void ScrollViewr_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e) {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight) {
                _controller?.ShowNextPage();
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if ((bool)CheckboxOnlyFinalized.IsChecked) {
                _controller?.ShowOnlyFinalized();
            } else {
                _controller?.ShowSourceLibrary();
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => _controller?.ActualizeItemSizeAndCollumns();

        private void GridView_ItemClick(object sender, ItemClickEventArgs e) => _controller.OpenHqDetails(_isFinalized);

        private void Lether_Click(object sender, RoutedEventArgs e) {
            var btn = sender as Button;
            _controller.ShowLether(btn.Content as String);
        }

        private void CheckboxOnlyFinalized_Checked(object sender, RoutedEventArgs e) {
            _isFinalized = true;
            _controller?.ShowOnlyFinalized();
        }

        private void CheckboxOnlyFinalized_Unchecked(object sender, RoutedEventArgs e) {
            _isFinalized = false;
            _controller?.ShowSourceLibrary();
        }
    }
}
