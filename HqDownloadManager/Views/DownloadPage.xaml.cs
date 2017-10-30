using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DependencyInjectionResolver;
using HqDownloadManager.Controllers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.ViewModels;
using Page = System.Windows.Controls.Page;

namespace HqDownloadManager.Views {
    public partial class DownloadPage : PageControllerBase<DownloadController> {
        private bool _paused = false;

        public DownloadPage(DependencyInjection dependencyInjection) : base(dependencyInjection) {
            InitializeComponent();
        }

        private void BtnInitDownload_Click(object sender, RoutedEventArgs e) => Controller.Download();

        private void BtnPauseDownload_Click(object sender, RoutedEventArgs e) {
            _paused = !_paused;
            Controller.PauseResume(_paused);
        }
    }
}
