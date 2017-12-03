using HqDownloadManager.Controller;
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

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace HqDownloadManager.Views {
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MyLibraryPage : MyLibraryController {
        public MyLibraryPage() {
            this.InitializeComponent();
            Unloaded += OnUnloaded;
        }

        public override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            ShowReadings();
            ShowDownloads();
        }


        private void ItemClick(object sender, ItemClickEventArgs e) => OpenHqDetails<HqDetailsPage>();

        private void OnUnloaded(object sender, RoutedEventArgs e) => ClearLists();

        private void KeepReading_ItemClick(object sender, ItemClickEventArgs e) => KeepReading<HqReaderPage>(e.ClickedItem);

        private void Erase_Click(object sender, RoutedEventArgs e) {
            if (sender is MenuFlyoutItem menuFlyoutItem) {
                if (menuFlyoutItem.DataContext is Hq selected) {
                    DeleteHq(selected);
                }
            }
        }

        private void EraseAll_Click(object sender, RoutedEventArgs e) {
            if (sender is FrameworkElement menuFlyoutItem) {
                var lkjad = HqsDownloaded.SelectedItem;
                if (menuFlyoutItem.DataContext is Hq selected) {
                    DeleteHq(selected, true);
                }
            }
        }

        private void Follow_Click(object sender, RoutedEventArgs e) {

        }

        private void GetUpdates_Click(object sender, RoutedEventArgs e) {

        }

        private void HqsDownloaded_SelectionChanged(object sender, SelectionChangedEventArgs e) => OpenHqDetails<HqDetailsPage>();
    }
}
