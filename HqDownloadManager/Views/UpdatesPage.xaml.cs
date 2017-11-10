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
using DependencyInjectionResolver;
using HqDownloadManager.Controllers;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace HqDownloadManager.Views {
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class UpdatesPage : Page {
        private readonly UpdatesPageController _controller;
        public UpdatesPage() {
            this.InitializeComponent();
            _controller = new DependencyInjection().Resolve<UpdatesPageController>();
            Loaded += UpdatesPage_Loaded;
        }

        private void UpdatesPage_Loaded(object sender, RoutedEventArgs e) {
            _controller.Init();
            _controller.ShowHqUpdates();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => _controller?.ShowHqUpdates();

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => _controller ?.ActualizeItemSizeAndCollumns();

        private void GridView_ItemClick(object sender, ItemClickEventArgs e) => _controller.OpenHqDetails();
    }
}
