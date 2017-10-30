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

    public partial class HqDetailsPage : PageControllerBase<HqDetailsController> {
        private readonly Hq _hq;

        public HqDetailsPage(Hq hq, DependencyInjection dependencyInjection) : base(dependencyInjection) {
            InitializeComponent();
            _hq = hq;
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            if (Resources["Hq"] is HqDetailsViewModel hqResource) {
                hqResource.Hq = _hq;
            }
            Controller.Init();
        }

        private void AddAll_Click(object sender, RoutedEventArgs e) => Controller.AddToDownloadList(_hq);

        private void AddSelected_Click(object sender, RoutedEventArgs e) =>
            Controller.AddChaptersSelectedToDownload(_hq);

        private void FollowHq_Click(object sender, RoutedEventArgs e) => Controller.FollowHq(_hq);

        private void ReadManga_Click(object sender, RoutedEventArgs e) => Controller.OpenReader(_hq);

        private void ReadNow_OnClick(object sender, RoutedEventArgs e) => Controller.ReadNow(_hq);
    }
}
