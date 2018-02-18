using HqDownloadManager.Core;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqDownloadManager.Shared.Database;
using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.MainWindow;
using HqDownloadManager.Shared.ViewModel.Reader;
using HqDownloadManager.Shared.ViewModel.Shared;
using HqManager;
using HqManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using WPF.Tools.Navigation.Events;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Controller {
    public class HqReaderController : System.Windows.Controls.Page {
        protected DownloadManager _downloadManager;
        private SourceManager _sourceManager;
        protected EntryManager _entryManager;
        private UserContext _userContext;
        protected NotificationViewModel _notification;
        private ReaderViewModel _readerViewModel;
        private NavigationViewModel _pageTitleView;
        private System.Windows.Controls.ListView _readerView;
        private ScrollViewer _scroolOfReader;
        private ICollectionView _collectionView;

        public HqReaderController(
                 DownloadManager downloadManager,
                 SourceManager sourceManager,
                 EntryManager entryManager, 
                 UserContext userContext) {
            _downloadManager = downloadManager;
            _sourceManager = sourceManager;
            _entryManager = entryManager;
            _userContext = userContext;
            NavigationEventHub.Navigated += NavigationEventHub_Navigated;
            CoreEventHub.ProcessingProgress += CoreEventHub_ProcessingProgress;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _notification = ControlsHelper.FindResource<NotificationViewModel>("Notification");
            _pageTitleView = ControlsHelper.FindResource<NavigationViewModel>("NavigationView");
            _readerView = ControlsHelper.Find<System.Windows.Controls.ListView>("FlipViewReader");
            _scroolOfReader = _readerView.Find<ScrollViewer>().FirstOrDefault();
            _scroolOfReader.ScrollChanged += ScroolViwer_ViewChanged;
        }

        private void NavigationEventHub_Navigated(object sender, NavigationEventArgs e) {
            _readerViewModel = ControlsHelper.FindResource<ReaderViewModel>("ReaderControl");
            var collection = ControlsHelper.FindResource<CollectionViewSource>("Reader");
            if (e.ExtraContent != null && _readerViewModel != null) {
                var reader = e.ExtraContent as ReaderViewModel;
                _readerViewModel.Hq = reader.Hq;
                collection.Source = _readerViewModel.Hq.Chapters;
                _collectionView = collection?.View;
                _collectionView.MoveCurrentToPosition(reader.ActualChapterIndex);
                _readerViewModel.ActualChapter = reader.ActualChapter;
                _readerViewModel.ActualChapterIndex = reader.ActualChapterIndex;
                _readerViewModel.PreviousChapter = reader.PreviousChapter;
                _readerViewModel.NextChapter = reader.NextChapter;
                LoadChapter();
            }
        }

        private void ScroolViwer_ViewChanged(object sender, ScrollChangedEventArgs e) {
            var scroll = sender as ScrollViewer;
            var posi = scroll.HorizontalOffset;
            var width = _readerView.ActualWidth;
            var index = Convert.ToInt32(posi / width);
            _readerViewModel.ActualPage = index;
        }

        public void LoadChapter() {
            var index = _collectionView.CurrentPosition;
            var actualChap = _collectionView.CurrentItem as Chapter;
            if (actualChap.Pages == null || actualChap.Pages.Count == 0) {
                var source = _sourceManager.GetSourceFromLink(actualChap.Link);
                var chap = new Chapter();
                source.GetInfo(actualChap.Link, out chap);
                _readerViewModel.ActualChapter = chap;
            } else {
                _readerViewModel.ActualChapter = actualChap;
            }

            if (_readerViewModel.Hq.Chapters.Count > 1 && _readerViewModel.Hq.Chapters.Count < _readerViewModel.ActualChapterIndex) {
                var nextChap = _readerViewModel.Hq.Chapters[index + 1] as Chapter;
                if (nextChap.Pages == null || nextChap.Pages.Count == 0) {
                    var source = _sourceManager.GetSourceFromLink(nextChap.Link);
                    var chap = new Chapter();
                    source.GetInfo(nextChap.Link, out chap);
                    _readerViewModel.NextChapter = chap;
                } else {
                    _readerViewModel.NextChapter = nextChap;
                }
            }

            _scroolOfReader.ScrollToTop();
            _pageTitleView.PageTitle = _readerViewModel.ActualChapter.Title;
        }

        public void NextChapter() {
            if (_collectionView.MoveCurrentToNext()) {
                LoadNext();
                SaveStatus();
            }
        }

        public void PreviousChapter() {
            if (_collectionView.MoveCurrentToPrevious()) {
                LoadPrevious();
                SaveStatus();
            }
        }


        private void LoadNext() {
            _readerViewModel.PreviousChapter = _readerViewModel.ActualChapter;
            _readerViewModel.ActualChapterIndex++;
            LoadChapter();
        }

        private void LoadPrevious() {
            _readerViewModel.NextChapter = _readerViewModel.ActualChapter;
            _readerViewModel.ActualChapter = _readerViewModel.PreviousChapter;
            _readerViewModel.ActualChapterIndex = _collectionView.CurrentPosition;
            var index = _readerViewModel.ActualChapterIndex;
            if (index >= 0) {
                var previousChap = _readerViewModel.Hq.Chapters[index] as Chapter;
                if (previousChap.Pages == null || previousChap.Pages.Count == 0) {
                    var source = _sourceManager.GetSourceFromLink(previousChap.Link);
                    var chap = new Chapter();
                    source.GetInfo(previousChap.Link, out chap);
                    _readerViewModel.PreviousChapter = chap;
                } else {
                    _readerViewModel.PreviousChapter = previousChap;
                }
            } else {
                _readerViewModel.PreviousChapter = null;
            }

            _scroolOfReader.ScrollToTop();
            _pageTitleView.PageTitle = _readerViewModel.ActualChapter.Title;
        }

        public void LoadSelection() {
            _collectionView.MoveCurrentToPosition(_readerViewModel.ActualChapterIndex);
            _pageTitleView.PageTitle = _readerViewModel.ActualChapter.Title;
            LoadChapter();
        }

        private void SaveStatus() {
            var hq = _readerViewModel.Hq;
            var id = _userContext.Reader.Save(_readerViewModel);
            _readerViewModel.Id = Convert.ToInt32(id);
            var readerHistpry = new ReaderHistory { Date = DateTime.Now, Reader = _readerViewModel };
            if (_userContext.HqEntry.Find().Where(x => x.Hq == hq).Execute().FirstOrDefault() is HqEntry entry) {
                entry.LastChapterRead = _readerViewModel.ActualChapter.Title;
                entry.ReadStatus = ReadStatus.LENDO.ToString();
                _userContext.HqEntry.Update(entry);
            }else {
                var hqEntry = new HqEntry {
                    Hq = hq, ReadStatus = ReadStatus.LENDO.ToString(), LastChapterRead = _readerViewModel.ActualChapter.Title
            };
                _userContext.HqEntry.Save(hqEntry);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void KeyWords() {
            if (Keyboard.IsKeyDown(Key.C)) {
                _readerViewModel.ControlsVisibility = !_readerViewModel.ControlsVisibility;
            }
            if (Keyboard.IsKeyDown(Key.N)) {
                _readerViewModel.DetailsVisibility = !_readerViewModel.DetailsVisibility;
            }
        }

        private void CoreEventHub_ProcessingProgress(object sender, ProcessingEventArgs ev) {
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    if (_notification != null) {
                        _notification.Message = ev.StateMessage;
                    }
                });
            });
        }
    }
}
