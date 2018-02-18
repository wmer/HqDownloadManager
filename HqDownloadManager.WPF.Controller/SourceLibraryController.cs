using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqDownloadManager.Shared.Database;
using HqDownloadManager.Shared.ViewModel.SourceLibrary;
using HqManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WPF.Tools.Navigation;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Controller {
    public class SourceLibraryController : SourceControllerBase {
        protected SourceLibraryViewModel _sourceLibraries;

        public SourceLibraryController(
                SourceManager sourceManager,
                ConfigurationContext configurationContext,
                DownloadManager downloadManager,
                EntryManager entryManage,
                NavigationManager navigationManager) : base(
                    sourceManager,
                    configurationContext,
                    entryManage,
                    downloadManager,
                    navigationManager) { }

        public override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            _sourceLibraries = ControlsHelper.FindResource<SourceLibraryViewModel>("Libraries");

            ShowLibrary();
        }

        public override void HqStatusChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedStatus" && _hqStatusView.SelectedStatus != "Add to...") {
                _hqStatusView.Visibility = true;
            }
        }

        public override void SelectedSourceChanged(object sender, PropertyChangedEventArgs e) {
            base.SelectedSourceChanged(sender, e);
            ShowLibrary();
        }

        public void ShowHqFinalized() => ShowLibrary(_actualSource.GetFinalizedPage);
        public void ShowLibrary() => ShowLibrary(_actualSource.GetLibrary);

        public void ShowLibraryWithLether(string lether) {
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    _notification.Visibility = true;
                });

                var library = new LibraryPage();
                _actualSource = _actualSource.GetLetherPage(lether, out library);
                Dispatcher.Invoke(() => {
                    _sourceLibraries.HqLibrary = new ObservableCollection<Hq>(library.Hqs); ;
                    _notification.Visibility = false;
                });
            });
        }

        public void OnScrollChanged(object sender, ScrollChangedEventArgs e) {
            if (e.VerticalChange != 0) {
                var scrollViewer = (ScrollViewer)e.OriginalSource;
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight) {
                    Task.Run(() => {
                        Dispatcher.Invoke(() => {
                            _notification.Visibility = true;
                        });

                        var library = new LibraryPage();
                        _actualSource = _actualSource.NextLibraryPage(out library);
                        Dispatcher.Invoke(() => {
                            foreach (var hq in library.Hqs) {
                                _sourceLibraries.HqLibrary.Add(hq);
                            }
                            _notification.Visibility = false;
                        });
                    });
                }
            }
        }

        private void ShowLibrary(LibraryOperation method) {
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    _notification.Visibility = true;
                });
                var library = GetHqsFromLibrary(method);
                Dispatcher.Invoke(() => {
                    _sourceLibraries.HqLibrary = library;
                    _notification.Visibility = false;
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
            _hqStatusView.Visibility = false;
        }
    }
}
