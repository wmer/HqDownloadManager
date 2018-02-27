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
        private readonly object _lock = new object();
        private ReaderHistory _actualReaderHistory;
        private int _actualPage;

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
                _readerViewModel.ActualPage = reader.ActualPage;
                _readerViewModel.NextChapter = reader.NextChapter;
                LoadChapter();
            }
        }

        private void ScroolViwer_ViewChanged(object sender, ScrollChangedEventArgs e) {
            var listViewIntem = _readerView.Find<System.Windows.Controls.ListViewItem>().FirstOrDefault();
            var scroll = sender as ScrollViewer;
            var posi = scroll.VerticalOffset;
            var height = listViewIntem.ActualHeight;
            var index = Convert.ToInt32(posi / height);
            if (index != _actualPage) {
                _actualPage = index;
                _readerViewModel.ActualPage = index;
                SavePage();
            }
        }

        public void LoadChapter() {
            Task.Run(() => {
                int index = 0;
                Chapter actualChap = new Chapter();
                Dispatcher.Invoke(() => {
                    _notification.Visibility = true;
                    index = _collectionView.CurrentPosition;
                    actualChap = _collectionView.CurrentItem as Chapter;
                });
                if (actualChap.Pages == null || actualChap.Pages.Count == 0) {
                    var source = _sourceManager.GetSourceFromLink(actualChap.Link);
                    var chap = new Chapter();
                    source.GetInfo(actualChap.Link, out chap);
                    Dispatcher.Invoke(() => {
                        _readerViewModel.ActualChapter = chap;
                    });
                } else {
                    Dispatcher.Invoke(() => {
                        _readerViewModel.ActualChapter = actualChap;
                    });
                }

                if (_readerViewModel.Hq.Chapters.Count > 1 && _readerViewModel.Hq.Chapters.Count < _readerViewModel.ActualChapterIndex) {
                    var nextChap = new Chapter();
                    Dispatcher.Invoke(() => {
                        nextChap = _readerViewModel.Hq.Chapters[index + 1] as Chapter;
                    });
                    if (nextChap.Pages == null || nextChap.Pages.Count == 0) {
                        var source = _sourceManager.GetSourceFromLink(nextChap.Link);
                        var chap = new Chapter();
                        source.GetInfo(nextChap.Link, out chap);
                        Dispatcher.Invoke(() => _readerViewModel.NextChapter = chap);
                    } else {
                        Dispatcher.Invoke(() => _readerViewModel.NextChapter = nextChap);
                    }
                }


                Dispatcher.Invoke(() => {
                    _readerView.SelectedIndex = _readerViewModel.ActualPage;
                    _readerView.ScrollIntoView(_readerView.SelectedItem);
                    _pageTitleView.PageTitle = _readerViewModel.ActualChapter.Title;
                    _notification.Visibility = false;
                    SaveChapter();
                });
            });
        }

        public void NextChapter() {
            if (_collectionView.MoveCurrentToNext()) {
                _readerViewModel.PreviousChapter = _readerViewModel.ActualChapter;
                _readerViewModel.ActualChapterIndex++;
                _readerViewModel.ActualPage = 0;
                _actualPage = 0;
                LoadChapter();
            }
        }

        public void PreviousChapter() {
            if (_collectionView.MoveCurrentToPrevious()) {
                LoadPrevious();
            }
        }

        private void LoadPrevious() {
            Task.Run(() => {
                var index = 0;
                var previousChap = new Chapter();
                Dispatcher.Invoke(() => {
                    _notification.Visibility = true;
                    _readerViewModel.NextChapter = _readerViewModel.ActualChapter;
                    _readerViewModel.ActualChapter = _readerViewModel.PreviousChapter;
                    _readerViewModel.ActualChapterIndex = _collectionView.CurrentPosition;
                    index = _readerViewModel.ActualChapterIndex;
                });
                if (index >= 0) {
                    Dispatcher.Invoke(() => {
                        previousChap = _readerViewModel.Hq.Chapters[index] as Chapter;
                    });
                    if (previousChap.Pages == null || previousChap.Pages.Count == 0) {
                        var source = _sourceManager.GetSourceFromLink(previousChap.Link);
                        var chap = new Chapter();
                        source.GetInfo(previousChap.Link, out chap);
                        Dispatcher.Invoke(() => _readerViewModel.PreviousChapter = chap);
                    } else {
                        Dispatcher.Invoke(() => _readerViewModel.PreviousChapter = previousChap);
                    }
                } else {
                    Dispatcher.Invoke(() => _readerViewModel.PreviousChapter = null);
                }
                Dispatcher.Invoke(() => {
                    _scroolOfReader.ScrollToTop();
                    _pageTitleView.PageTitle = _readerViewModel.ActualChapter.Title;
                    _notification.Visibility = false;
                    SaveChapter();
                });
            });
        }

        public void LoadSelection() {
            _readerViewModel.ActualPage = 0;
            _actualPage = 0;
            _collectionView.MoveCurrentToPosition(_readerViewModel.ActualChapterIndex);
            _pageTitleView.PageTitle = _readerViewModel.ActualChapter.Title;
            LoadChapter();
        }

        private void SavePage() {
            Task.Run(() => {
                lock (_lock) {
                    Dispatcher.Invoke(() => {
                        var id = _userContext.Reader.Save(_readerViewModel);
                        _readerViewModel.Id = Convert.ToInt32(id);
                    });
                    if (_actualReaderHistory != null) {
                        _userContext.ReaderHistory.Update(x => new { x.Date, x.Reader }, DateTime.Now, _readerViewModel)
                                                  .Where(x => x.Id == _actualReaderHistory.Id)
                                                  .Execute();
                    } else {
                        if (_userContext.ReaderHistory.Find()
                                         .Where(x => x.Link == _readerViewModel.Hq.Link)
                                         .Execute()
                                         .FirstOrDefault(x =>
                                           x.Reader.ActualChapterIndex == _readerViewModel.ActualChapterIndex
                                         ) is ReaderHistory readerHistory) {
                            _actualReaderHistory = readerHistory;
                            _userContext.ReaderHistory.Update(x => new { x.Date, x.Reader }, DateTime.Now, _readerViewModel)
                                                      .Where(x => x.Id == _actualReaderHistory.Id)
                                                      .Execute();

                        } else {
                            _actualReaderHistory = new ReaderHistory { Link = _readerViewModel.Hq.Link, Date = DateTime.Now, Reader = _readerViewModel };
                            _actualReaderHistory.Id = Convert.ToInt32(_userContext.ReaderHistory.Save(_actualReaderHistory));
                        }
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            });
        }

        private void SaveChapter() {
            var hq = _readerViewModel.Hq;
            var link = hq.Link;
            if (hq.Id == 0 && !string.IsNullOrEmpty(hq.Link) && !(_userContext.Hq.Find().Where(x => x.Link == link).Execute().FirstOrDefault() is Hq)) {
                hq.IsDetailedInformation = false;
                hq.Id = Convert.ToInt32(_userContext.Hq.Save(hq));
            } else if (hq.Id == 0 && !string.IsNullOrEmpty(hq.Link) && _userContext.Hq.Find().Where(x => x.Link == link).Execute().FirstOrDefault() is Hq hqResult) {
                hq = hqResult;
            }
            var id = _userContext.Reader.Save(_readerViewModel);
            _readerViewModel.Id = Convert.ToInt32(id);
            _actualReaderHistory = new ReaderHistory { Link = hq.Link, Date = DateTime.Now, Reader = _readerViewModel };
            if (_userContext.HqEntry.Find().Where(x => x.Hq == hq).Execute().FirstOrDefault() is HqEntry entry) {
                entry.LastChapterRead = _readerViewModel.ActualChapter.Title;
                _userContext.HqEntry.Update(entry);
            } else {
                var hqEntry = new HqEntry {
                    Hq = hq, ReadStatus = ReadStatus.LENDO.ToString(), LastChapterRead = _readerViewModel.ActualChapter.Title
                };
                _userContext.HqEntry.Save(hqEntry);
            }
            _actualReaderHistory.Id = Convert.ToInt32(_userContext.ReaderHistory.Save(_actualReaderHistory));
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
