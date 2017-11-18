using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Utils;
using HqDownloadManager.Controller.Models;
using HqDownloadManager.Controller.ViewModel.MainPage;
using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Core.Models;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace HqDownloadManager.Controller {
    public class HqReaderController : ControllerBase {
        private ReaderViewModel _readerViewModel;
        private ICollectionView _collectionView;
        private NavigationViewModel _pageTitleView;
        private FlipView _flipView;

        public HqReaderController(DependencyInjection dependencyInjection) : base(dependencyInjection) {
        }

        public async Task InitReader(Hq hq) {
            _pageTitleView = ControlsHelper.FindResource<NavigationViewModel>("TitleViewModel");
            _flipView = ControlsHelper.Find<FlipView>("itemFlipView");
            _readerViewModel = ControlsHelper.FindResource<ReaderViewModel>("ReaderControl");
            _readerViewModel.Hq = hq;
            var collection = ControlsHelper.FindResource<CollectionViewSource>("Reader");
            collection.Source = hq.Chapters;
            _collectionView = collection?.View;
            if (UserLibrary.UserReadings.FindOne(hq.Link) is UserReading userReading) {
                var reader = userReading.HqReaderViewModel.ToObject<ReaderViewModel>();
                UserLibrary.UserReadings.Delete(userReading);
                ContentDialog deleteFileDialog = new ContentDialog {
                    Title = "Continuar lendo?",
                    Content = "Deseja continuar ou iniciar uma nova leitura?",
                    PrimaryButtonText = "Continuar",
                    CloseButtonText = "Reiniciar"
                };

                ContentDialogResult result = await deleteFileDialog.ShowAsync();
                if (result == ContentDialogResult.Primary) {
                    _readerViewModel.ActualChapter = reader.ActualChapter;
                    _readerViewModel.NextChapter = reader.NextChapter;
                    _readerViewModel.PreviousChapter = reader.PreviousChapter;
                    _readerViewModel.ActualPage = reader.ActualPage;
                    _readerViewModel.ActualChapterIndex = reader.ActualChapterIndex;

                    _collectionView.MoveCurrentToPosition(reader.ActualChapterIndex);
                    _flipView.SelectedItem = _readerViewModel.ActualChapter.Pages[reader.ActualPage];
                    _pageTitleView.Title = reader.ActualChapter.Title;
                } else {
                    LoadChapter();
                }               
            } else {
                LoadChapter();
            }
        }

        public void LoadChapter() {
            var index = _collectionView.CurrentPosition;
            var actualChap = _collectionView.CurrentItem as Chapter;
            if (actualChap.Pages == null || actualChap.Pages.Count == 0) {
                _readerViewModel.ActualChapter = SourceManager.GetInfo(actualChap.Link) as Chapter;
            } else {
                _readerViewModel.ActualChapter = actualChap;
            }

            if (_collectionView.Count > 1 && _collectionView.Count < _readerViewModel.ActualChapterIndex) {
                var nextChap = _collectionView[index + 1] as Chapter;
                if (nextChap.Pages == null || nextChap.Pages.Count == 0) {
                    _readerViewModel.NextChapter = SourceManager.GetInfo(nextChap.Link) as Chapter;
                } else {
                    _readerViewModel.NextChapter = nextChap;
                }
            } else {
                _readerViewModel.NextChapter = null;
            }

            _pageTitleView.Title = actualChap.Title;
            SaveStatus();
        }

        public void ShowHideControllers() {
            if (_readerViewModel.ControlsVisibility == Visibility.Collapsed) {
                _readerViewModel.ControlsVisibility = Visibility.Visible;
            } else {
                _readerViewModel.ControlsVisibility = Visibility.Collapsed;
            }
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

        private void LoadNext() {
            _readerViewModel.PreviousChapter = _readerViewModel.ActualChapter;
            _readerViewModel.ActualChapterIndex++;
            LoadChapter();
        }

        private void LoadPrevious() {
            _readerViewModel.NextChapter = _readerViewModel.ActualChapter;
            _readerViewModel.ActualChapter = _readerViewModel.PreviousChapter;
            _readerViewModel.ActualChapterIndex--;
            var index = _readerViewModel.ActualChapterIndex - 1;
            if (index >= 0) {
                var previousChap = _collectionView[index] as Chapter;
                if (previousChap.Pages == null || previousChap.Pages.Count == 0) {
                    _readerViewModel.PreviousChapter = SourceManager.GetInfo(previousChap.Link) as Chapter;
                } else {
                    _readerViewModel.PreviousChapter = previousChap;
                }
            } else {
                _readerViewModel.PreviousChapter = null;
            }

            _pageTitleView.Title = _readerViewModel.ActualChapter?.Title;
            SaveStatus();
        }

        public void ActualizeactualPage() {
            _readerViewModel.ActualPage = _flipView.SelectedIndex;
            SaveStatus();
        }

        private void SaveStatus() {
            var userReading = new UserReading {
                Link = _readerViewModel.Hq.Link,
                Date = DateTime.Now,
                HqReaderViewModel = _readerViewModel.ToBytes()
            };

            if (UserLibrary.UserReadings.FindOne(_readerViewModel.Hq.Link) != null) {
                UserLibrary.UserReadings.Update(userReading);
            } else {
                UserLibrary.UserReadings.Save(userReading);
            }
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
    }
}
