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
using DependencyInjectionResolver;
using HqDownloadManager.Controllers;

namespace HqDownloadManager.Views {
    public partial class SourceLibraryPage : PageControllerBase<SourceLibraryController> {
        private bool _isFinalized = false;

        public SourceLibraryPage(DependencyInjection dependencyInjection) : base(dependencyInjection) {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            if ((bool)CheckboxOnlyFinalized.IsChecked) {
                Controller?.ShowOnlyFinalized();
            } else {
                Controller.ShowSourceLibrary();
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) =>
            Controller?.ActualizeItemSizeAndCollumns();

        private void SourceHq_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Controller?.ShowSourceLibrary();
            if (_isFinalized) {
                Controller?.ShowOnlyFinalized();
            }
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) =>
            Controller.SetTimesOfClick(sender, e);

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e) =>
            Controller.Click(sender, e, () => {
                Controller.OpenHqDetails(_isFinalized);
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

        private void HqList_OnScrollChanged(object sender, ScrollChangedEventArgs e) {
            if (e.VerticalChange != 0) {
                var scrollViewer = (ScrollViewer)e.OriginalSource;
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight) {
                    Task.Run(() => {
                        Controller.ShowNextPage();
                    });
                }
            }
        }

        private void CheckboxOnlyFinalized_OnChecked(object sender, RoutedEventArgs e) {
            _isFinalized = true;
            Controller?.ShowOnlyFinalized();
        }


        private void CheckboxOnlyFinalized_OnUnchecked(object sender, RoutedEventArgs e) {
            _isFinalized = false;
            Controller?.ShowSourceLibrary();
        }

        private void AddToDownload_OnClick(object sender, RoutedEventArgs e) => Controller.AddInDownloadList(_isFinalized);
    }
}
