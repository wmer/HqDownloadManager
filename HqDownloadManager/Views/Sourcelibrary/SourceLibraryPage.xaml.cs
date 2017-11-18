using HqDownloadManager.Controller.ViewsController;
using HqDownloadManager.Views.DetailsPage;
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

namespace HqDownloadManager.Views.Sourcelibrary {
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class SourceLibraryPage : SourceLibraryPageBase {
        private bool _isFinalized = false;

        public SourceLibraryPage() {
            this.InitializeComponent();
            Unloaded += OnUnloaded;
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);

            var scrollViewr = HqlistGrid.GetFirstDescendantOfType<ScrollViewer>();
            scrollViewr.ViewChanged += ScrollViewr_ViewChanged;

            if ((bool)CheckboxOnlyFinalized.IsChecked) {
                Controller?.ShowOnlyFinalized();
            } else {
                Controller.ShowSourceLibrary();
            }
        }

        private void ScrollViewr_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e) {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight) {
                Controller?.ShowNextPage();
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if ((bool)CheckboxOnlyFinalized.IsChecked) {
                Controller?.ShowOnlyFinalized();
            } else {
                Controller?.ShowSourceLibrary();
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => Controller?.ActualizeItemSizeAndCollumns();

        private void GridView_ItemClick(object sender, ItemClickEventArgs e) => Controller.OpenHqDetails<HqDetailsPage>(_isFinalized);

        private void Lether_Click(object sender, RoutedEventArgs e) {
            var btn = sender as Button;
            Controller.ShowLether(btn.Content as String);
        }

        private void CheckboxOnlyFinalized_Checked(object sender, RoutedEventArgs e) {
            _isFinalized = true;
            Controller?.ShowOnlyFinalized();
        }

        private void CheckboxOnlyFinalized_Unchecked(object sender, RoutedEventArgs e) {
            _isFinalized = false;
            Controller?.ShowSourceLibrary();
        }

        private async void AddDownload_Click(object sender, RoutedEventArgs e) => await Controller.AddToDownloadList();

        private async void Follow_Click(object sender, RoutedEventArgs e) => await Controller.FollowHq();

        private void OnUnloaded(object sender, RoutedEventArgs e) => Controller.ClearList();
    }
}
