using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using Utils;
using HqDownloadManager.Controller.Models;
using HqDownloadManager.Controller.ViewModel.MainPage;
using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Core.Models;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;

namespace HqDownloadManager.Controller {
    public class HqReaderController : ControllerBase {
        private ReaderViewModel _readerViewModel;
        private ICollectionView _collectionView;
        private NavigationViewModel _pageTitleView;
        private FlipView _flipView;
        private CollectionViewSource _collection;

        public HqReaderController() : base() {
        }

        public void InitControls() {
            _pageTitleView = ControlsHelper.FindResource<NavigationViewModel>("TitleViewModel");
            _flipView = ControlsHelper.Find<FlipView>("itemFlipView");
            _readerViewModel = ControlsHelper.FindResource<ReaderViewModel>("ReaderControl");
            _collection = ControlsHelper.FindResource<CollectionViewSource>("Reader");
        }

        public async Task InitReader(Hq hq) {
            _readerViewModel.Hq = hq;
            _collection.Source = hq.Chapters;
            _collectionView = _collection?.View;
            if (UserLibrary.UserReadings.FindOne(hq.Link) is UserReading readings) {
                var reader = readings.Reading.ToObject<ReaderViewModel>();
                ContentDialogResult result = await OpenDialog();
                if (result == ContentDialogResult.Primary) {
                    OpenFromCache(reader);
                } else {
                    LoadChapter();
                }               
            } else {
                LoadChapter();
            }
        }

        public async Task OpenReader(ReaderViewModel reader) {
            _collection.Source = reader.Hq.Chapters;
            _collectionView = _collection?.View;
            if (reader.ActualPage > 1) {
                ContentDialogResult result = await OpenDialog();
                if (result == ContentDialogResult.Primary) {
                    OpenFromCache(reader);
                } else {
                    _collectionView.MoveCurrentToPosition(reader.ActualChapterIndex);
                    LoadChapter();
                }
            }else {
                OpenFromCache(reader);
            }
        }

        public void LoadChapter() {
            var index = _collectionView.CurrentPosition;
            _readerViewModel.ActualChapterIndex = index;
             var actualChap = _collectionView.CurrentItem as Chapter;
            if (actualChap.Pages == null || actualChap.Pages.Count == 0) {
                _readerViewModel.ActualChapter = SourceManager.GetInfo<Chapter>(actualChap.Link);
                var hq = _readerViewModel.Hq;
                hq.Chapters[index] = _readerViewModel.ActualChapter;
                UserLibrary.Hq.Update(x => new { x.Hq }, hq.ToBytes())
                                                         .Where(x => x.Link == hq.Link).Execute();
            } else {
                _readerViewModel.ActualChapter = actualChap;
            }

            if (_collectionView.Count > 1 && index < _collectionView.Count -1) {
                var nextChap = _collectionView[index + 1] as Chapter;
                if (nextChap.Pages == null || nextChap.Pages.Count == 0) {
                    _readerViewModel.NextChapter = SourceManager.GetInfo<Chapter>(nextChap.Link);
                    var hq = _readerViewModel.Hq;
                    hq.Chapters[index + 1] = _readerViewModel.NextChapter;
                    UserLibrary.Hq.Update(x => new { x.Hq }, hq.ToBytes())
                                                             .Where(x => x.Link == hq.Link).Execute();
                } else {
                    _readerViewModel.NextChapter = nextChap;
                }
            } else {
                _readerViewModel.NextChapter = null;
            }

            _pageTitleView.Title = actualChap.Title;
            SaveStatus(_readerViewModel.Hq.Link);
        }

        private void LoadPrevious() {
            var index = _collectionView.CurrentPosition;
            _readerViewModel.ActualChapterIndex = index;
            _readerViewModel.NextChapter = _readerViewModel.ActualChapter;
            if (_readerViewModel.PreviousChapter.Pages == null || _readerViewModel.PreviousChapter.Pages.Count == 0) {
                _readerViewModel.PreviousChapter = SourceManager.GetInfo<Chapter>(_readerViewModel.PreviousChapter.Link);
            }
            _readerViewModel.ActualChapter = _readerViewModel.PreviousChapter;
            if (index > 0) {
                var previousChap = _collectionView[index - 1] as Chapter;
                if (previousChap.Pages == null || previousChap.Pages.Count == 0) {
                    _readerViewModel.PreviousChapter = SourceManager.GetInfo<Chapter>(previousChap.Link);
                } else {
                    _readerViewModel.PreviousChapter = previousChap;
                }
            } else {
                _readerViewModel.PreviousChapter = null;
            }

            _pageTitleView.Title = _readerViewModel.ActualChapter?.Title;
            SaveStatus(_readerViewModel.Hq.Link);
        }

        public void NextChapter() {
            if (_collectionView.MoveCurrentToNext()) {
                LoadNext();
            }
        }

        public void PreviousChapter() {
            if (_collectionView.MoveCurrentToPrevious()) {
                LoadPrevious();
            }
        }

        public void ShowHideControllers() {
            if (_readerViewModel.ControlsVisibility == Visibility.Collapsed) {
                _readerViewModel.ControlsVisibility = Visibility.Visible;
            } else {
                _readerViewModel.ControlsVisibility = Visibility.Collapsed;
            }
        }

        private void LoadNext() {
            _readerViewModel.PreviousChapter = _readerViewModel.ActualChapter;
            _readerViewModel.ActualChapterIndex++;
            LoadChapter();
        }

        public void ActualizeactualPage() {
            _readerViewModel.ActualPage = _flipView.SelectedIndex;
            SaveStatus(_readerViewModel.ActualChapter.Link);
        }

        private void SaveStatus(string link) {
            var userReading = new UserReading {
                Link = link, Date = DateTime.Now, Reading = _readerViewModel.ToBytes()
            };

            if (UserLibrary.UserReadings.FindOne(link) != null) {
                UserLibrary.UserReadings.Update(userReading);
            } else {
                UserLibrary.UserReadings.Save(userReading);
            }
            GC.Collect();
        }

        private async Task<ContentDialogResult> OpenDialog() {
            ContentDialog deleteFileDialog = new ContentDialog {
                Title = "Continuar lendo?",
                Content = "Deseja continuar ou iniciar uma nova leitura?",
                PrimaryButtonText = "Continuar",
                CloseButtonText = "Reiniciar"
            };

            return await deleteFileDialog.ShowAsync();
        }

        private void OpenFromCache(ReaderViewModel reader) {
            _readerViewModel.Hq = reader.Hq;
            _readerViewModel.ActualChapter = reader.ActualChapter;
            _readerViewModel.NextChapter = reader.NextChapter;
            _readerViewModel.PreviousChapter = reader.PreviousChapter;
            _readerViewModel.ActualPage = reader.ActualPage;
            _readerViewModel.ActualChapterIndex = reader.ActualChapterIndex;

            _collectionView.MoveCurrentToPosition(reader.ActualChapterIndex);
            _flipView.SelectedItem = _readerViewModel.ActualChapter.Pages[reader.ActualPage];
            _pageTitleView.Title = reader.ActualChapter.Title;
        }

        public void FullSCream() {
            ApplicationView view = ApplicationView.GetForCurrentView();

            bool isInFullScreenMode = view.IsFullScreenMode;

            if (isInFullScreenMode) {
                view.ExitFullScreenMode();
                _pageTitleView.CompactModeMinWidth = 250;
            } else {
                view.TryEnterFullScreenMode();
                _pageTitleView.CompactModeMinWidth = 1940;
            }
        }

        public async Task NormalizeZoom(ScrollViewer scrollViewer, DoubleTappedRoutedEventArgs e) {
            var doubleTapPoint = e.GetPosition(scrollViewer);

            if (scrollViewer.ZoomFactor != 1) {
                scrollViewer.ZoomToFactor(1);
            } else if (scrollViewer.ZoomFactor == 1) {
                scrollViewer.ZoomToFactor(2);

                var dispatcher = Window.Current.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.High, () => {
                    scrollViewer.ScrollToHorizontalOffset(doubleTapPoint.X);
                    scrollViewer.ScrollToVerticalOffset(doubleTapPoint.Y);
                });
            }
        }
    }
}
