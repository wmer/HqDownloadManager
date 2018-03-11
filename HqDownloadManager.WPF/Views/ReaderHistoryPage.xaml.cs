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

namespace HqDownloadManager.WPF.Views {
    /// <summary>
    /// Interação lógica para ReaderHistoryPage.xam
    /// </summary>
    public partial class ReaderHistoryPage : Page {
        private ReadingHistoryController _controller;
        public ReaderHistoryPage(ReadingHistoryController controller) {
            _controller = controller;
            InitializeComponent();
            this.Loaded += _controller.OnLoaded;
        }
    }
}
