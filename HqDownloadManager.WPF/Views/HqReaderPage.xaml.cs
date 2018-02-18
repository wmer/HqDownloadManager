using HqDownloadManager.WPF.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Views {
    /// <summary>
    /// Interação lógica para HqReaderPage.xam
    /// </summary>
    public partial class HqReaderPage : Page {
        private HqReaderController _controller;

        public HqReaderPage(HqReaderController controller) {
            _controller = controller;
            this.InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }

        private void BtnCloseDetails_Click(object sender, RoutedEventArgs e) => DetailsManga.Visibility = Visibility.Collapsed;

        private void Page_KeyDown(object sender, KeyEventArgs e) => _controller.KeyWords();

        private void BtnToNextChapter_Click(object sender, RoutedEventArgs e) => _controller.NextChapter();

        private void BtnToPreviusChapter_Click(object sender, RoutedEventArgs e) => _controller.PreviousChapter();

        private void ChapterReading_SelectionChanged(object sender, SelectionChangedEventArgs e) => _controller.LoadSelection();
    }
}
