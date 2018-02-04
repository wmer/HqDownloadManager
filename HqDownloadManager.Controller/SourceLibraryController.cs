using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Controller.ViewModel.ComboBox;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Controller.ViewModel.SourceLibrary;
using HqDownloadManager.Controller.ViewModel.SourceUpdate;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Managers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using WinRTXamlToolkit.Controls.Extensions;

namespace HqDownloadManager.Controller {
    public class SourceLibraryController : SourceControllerBase {
        protected SourceLibraryViewModel _sourceLibraries;

        public SourceLibraryController(
                SourceManager sourceManager,
                ControlsHelper controllerHelper,
                DownloadManager downloadManager,
                EntryManager entryManage) : base(
                    sourceManager,
                    entryManage,
                    downloadManager,
                    controllerHelper) { }

        public override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            _sourceLibraries = _controllerHelper.FindResource<SourceLibraryViewModel>("Libraries");

            ShowLibrary();
        }

        public override void HqStatusChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedStatus" && _hqStatusView.SelectedStatus != "Add to...") {
                _hqStatusView.Visibility = Visibility.Visible;
            }
        }

        public override void SelectedSourceChanged(object sender, PropertyChangedEventArgs e) {
            base.SelectedSourceChanged(sender, e);
            ShowLibrary();
        }

        public void ShowHqFinalized() => ShowLibrary(_actualSource.GetFinalizedPage);
        public void ShowLibrary() => ShowLibrary(_actualSource.GetLibrary);

        public void ShowLibraryWithLether(string lether) {
            Task.Run(async () => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _notification.Visibility = Visibility.Visible;
                });

                var library = new LibraryPage();
                _actualSource = _actualSource.GetLetherPage(lether, out library);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _sourceLibraries.HqLibrary = new ObservableCollection<Hq>(library.Hqs); ;
                    _notification.Visibility = Visibility.Collapsed;
                });
            });
        }

        public void OnScroll(object sender, ScrollEventArgs e) {
            var bar = (ScrollBar)sender;
            if (e.NewValue >= bar.Maximum) {
                Task.Run(async () => {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        _notification.Visibility = Visibility.Visible;
                    });

                    var library = new LibraryPage();
                    _actualSource = _actualSource.NextLibraryPage(out library);
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        foreach (var hq in library.Hqs) {
                            _sourceLibraries.HqLibrary.Add(hq);
                        }
                        _notification.Visibility = Visibility.Collapsed;
                    });
                });
            }
        }

        private void ShowLibrary(LibraryOperation method) {
            Task.Run(async () => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _notification.Visibility = Visibility.Visible;
                });
                var library = GetHqsFromLibrary(method);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _sourceLibraries.HqLibrary = library;
                    _notification.Visibility = Visibility.Collapsed;
                });
            });
        }

        private ObservableCollection<Hq> GetHqsFromLibrary(LibraryOperation method) {
            var library = new LibraryPage();
            method.Invoke(out library);
            return new ObservableCollection<Hq>(library.Hqs);
        }

        public void SaveEntry() {
            var entry = _hqStatusView.Entry;
            _entryManager.SaveEntry(_hqStatusView.Entry);
            _hqStatusView.Visibility = Visibility.Collapsed;
        }
    }
}
