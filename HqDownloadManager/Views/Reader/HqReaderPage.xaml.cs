using HqDownloadManager.Controller.ViewsController;
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

namespace HqDownloadManager.Views.Reader {
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class HqReaderPage : ReaderPageBase {
        private Hq _hq;

        public HqReaderPage() {
            this.InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            Controller.InitReader(_hq);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            _hq = e.Parameter as Hq;
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == VirtualKey.Control) {
                Controller.ShowHideControllers();
            }
            if (e.Key == VirtualKey.Shift) {
                Controller.FullSCream();
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e) => Controller.PreviousChapter();

        private void Next_Click(object sender, RoutedEventArgs e) => Controller.NextChapter();

        private void itemFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e) => Controller.ActualizeactualPage();
    }
}
