using HqDownloadManager.Controller;
using HqDownloadManager.Controller.ViewModel.HqDetails;
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
    public sealed partial class HqDetailsPage : HqDetailsController {
        private Hq _hq;

        public HqDetailsPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var hqModel = Resources["Hq"] as HqViewModel;
            _hq = e.Parameter as Hq;
            hqModel.Hq = _hq;
        }

        private void AddAll_Click(object sender, RoutedEventArgs e) => AddToDownloadList(_hq);

        private void AddSelected_Click(object sender, RoutedEventArgs e) => AddSelected(_hq);

        private void FollowHq_Click(object sender, RoutedEventArgs e) => FollowHq(_hq);

        private void ReadManga_Click(object sender, RoutedEventArgs e) => OpenReader<HqReaderPage>(_hq);

        private void ReadNow_Click(object sender, RoutedEventArgs e) => ReadNow<HqReaderPage>(_hq);
    }
}
