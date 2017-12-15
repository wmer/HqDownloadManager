using HqDownloadManager.Controller;
using HqDownloadManager.Controller.ViewModel.MyLibrary;
using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class DownloadDetails : DownloadDetailsController {
        private Hq _hq;

        public DownloadDetails() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            _hq = e.Parameter as Hq;
            LoadDetails(_hq);
        }

        private void BtnFollow_Click(object sender, RoutedEventArgs e) => FollowHq(_hq);

        private void BtnDelete_Click(object sender, RoutedEventArgs e) => DeleteHq<MyLibraryPage>(_hq, true);

        private void Chapters_ItemClick(object sender, ItemClickEventArgs e) => Read<HqReaderPage>((ReaderViewModel)e.ClickedItem);
    }
}
