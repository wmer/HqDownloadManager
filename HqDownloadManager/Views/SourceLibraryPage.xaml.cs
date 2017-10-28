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
    /// <summary>
    /// Interação lógica para SourceLibraryPage.xam
    /// </summary>
    public partial class SourceLibraryPage : Page {
        private readonly DependencyInjection _dependency;
        private SourceLibraryController _sourceLibraryController;
        private bool isFinalized = false;

        public SourceLibraryPage(DependencyInjection dependencyInjection) {
            InitializeComponent();
            _dependency = dependencyInjection;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            _sourceLibraryController = _dependency.Resolve<SourceLibraryController>();
            _sourceLibraryController.Init();
            if ((bool)CheckboxOnlyFinalized.IsChecked) {
                _sourceLibraryController?.ShowOnlyFinalized();
            } else {
                _sourceLibraryController.ShowSourceLibrary();
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) =>
            _sourceLibraryController?.ActualizeItemSizeAndCollumns();

        private void SourceHq_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            _sourceLibraryController?.ShowSourceLibrary();
            if (isFinalized) {
                _sourceLibraryController?.ShowOnlyFinalized();
             }
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) =>
            _sourceLibraryController.SetTimesOfClick(sender, e);

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e) =>
            _sourceLibraryController.Click(sender, e, () => {
                _sourceLibraryController.OpenHqDetails(isFinalized);
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
                        _sourceLibraryController.ShowNextPage();
                    });
                }
            }
        }

        private void CheckboxOnlyFinalized_OnChecked(object sender, RoutedEventArgs e) {
            isFinalized = true;
            _sourceLibraryController?.ShowOnlyFinalized();
        }


        private void CheckboxOnlyFinalized_OnUnchecked(object sender, RoutedEventArgs e) {
            isFinalized = false;
            _sourceLibraryController?.ShowSourceLibrary();
        }
    }
}
