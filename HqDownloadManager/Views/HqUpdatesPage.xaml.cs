using DependencyInjectionResolver;
using HqDownloadManager.Controllers;
using HqDownloadManager.Helpers;
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

namespace HqDownloadManager.Views {
    public partial class HqUpdatesPage : PageControllerBase<HqUpdatesController> {

        public HqUpdatesPage(DependencyInjection dependency) : base(dependency) {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            Controller.ShowHqUpdates();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) =>
            Controller?.ActualizeItemSizeAndCollumns();

        private void SourceHq_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            Controller?.ShowHqUpdates();

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) =>
            Controller.SetTimesOfClick(sender, e);

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e) =>
            Controller.Click(sender, e, () => {
                Controller.OpenHqDetails();
            });

        private void Grid_MouseEnter(object sender, MouseEventArgs e) {
            if ((sender as Grid)?.Children[0] is TextBox txtBox) {
                txtBox.Visibility = Visibility.Visible;
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e) {
            if ((sender as Grid)?.Children[0] is TextBox txtBox) {
                txtBox.Visibility = Visibility.Hidden;
            }
        }

        private void AddToDownload_OnClick(object sender, RoutedEventArgs e) => Controller.AddInDownloadList();
    }
}
