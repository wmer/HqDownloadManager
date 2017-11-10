using DependencyInjectionResolver;
using HqDownloadManager.Controllers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.ViewModels.HqDetails;
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
    public sealed partial class HqDetailsPage {
        private HqDetailsController _controller;
        private Hq _hq;

        public HqDetailsPage() {
            this.InitializeComponent();
            _controller = new DependencyInjection().Resolve<HqDetailsController>();
            Loaded += HqDetailsPage_Loaded;
        }

        private void HqDetailsPage_Loaded(object sender, RoutedEventArgs e) {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var hqModel = Resources["Hq"] as HqViewModel;
            _hq = e.Parameter as Hq;
            hqModel.Hq = _hq;
        }

        private void AddAll_Click(object sender, RoutedEventArgs e) {

        }

        private void AddSelected_Click(object sender, RoutedEventArgs e) => _controller.ReadNow(_hq);

        private void FollowHq_Click(object sender, RoutedEventArgs e) => _controller.FollowHq(_hq);

        private void ReadManga_Click(object sender, RoutedEventArgs e) => _controller.OpenReader(_hq);

        private void ReadNow_Click(object sender, RoutedEventArgs e) => _controller.ReadNow(_hq);
    }
}
