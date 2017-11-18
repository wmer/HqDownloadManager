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

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace HqDownloadManager.Views.Updates {
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class UpdatesPage : UpdatesPageBase {
        public UpdatesPage() {
            this.InitializeComponent();
            Unloaded += OnUnloaded;
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            Controller.ShowHqUpdates();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => Controller?.ShowHqUpdates();

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => Controller?.ActualizeItemSizeAndCollumns();

        private async void GridView_ItemClick(object sender, ItemClickEventArgs e) => await Controller.OpenHqDetails<HqDetailsPage>();

        private async void AddDownload_Click(object sender, RoutedEventArgs e) => await Controller.AddToDownloadList();

        private async void Follow_Click(object sender, RoutedEventArgs e) => await Controller.FollowHq();

        private void OnUnloaded(object sender, RoutedEventArgs e) => Controller.ClearList();
    }
}
