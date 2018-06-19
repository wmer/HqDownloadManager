using HqDownloadManager.Models;
using HqDownloadManager.WPF.Commands;
using HqDownloadManager.WPF.Databases;
using HqDownloadManager.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;
using WPF.Tools.Navigation.Events;

namespace HqDownloadManager.WPF.ViewModels {
    public class DownloadDetailsViewModel : ViewModelBase {
        private ReaderContext _readerContext;
        private OpenReaderCommand _openReaderCommand;

        private DownloadedHq downloadedHq;

        public DownloadedHq DownloadedHq {
            get { return downloadedHq; }
            set {
                downloadedHq = value;
                OnPropertyChanged("DownloadedHq");
            }
        }

        private ObservableCollection<ChapterReadingProgress> _chapterProgressHistory;
        public ObservableCollection<ChapterReadingProgress> ChapterProgressHistory {
            get { return _chapterProgressHistory; }
            set {
                _chapterProgressHistory = value;
                OnPropertyChanged("ChapterProgressHistory");
            }
        }

        private DownloadedChapter _selectedChapter;

        public DownloadedChapter SelectedChapter {
            get { return _selectedChapter; }
            set {
                _selectedChapter = value;
                OnPropertyChanged("SelectedChapter");
                _openReaderCommand.RaiseCanExecuteChanged();
            }
        }

        private ChapterReadingProgress _selectedHistory;

        public ChapterReadingProgress SelectedHistpry {
            get { return _selectedHistory; }
            set {
                _selectedHistory = value;
                OnPropertyChanged("SelectedHistpry");
            }
        }

        public DelegateCommand<DownloadDetailsViewModel> OpenReader {
            get { return _openReaderCommand.Command; }
        }

        public DownloadDetailsViewModel(
                    ReaderContext readerContext,
                    OpenReaderCommand openReaderCommand) {
            _readerContext = readerContext;
            _openReaderCommand = openReaderCommand;
            NavigationEventHub.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e) {
            var downloadedHq = e.ExtraContent;
            if (downloadedHq[0] is DownloadedHq dw) {
                DownloadedHq = dw;
                //var chapterProgress = _readerContext.ChapterReadingProgress
                //                                    .Find()
                //                                    .Where(x => x.HqLocation == DownloadedHq.Location)
                //                                    .Execute();
                //ChapterProgressHistory = new ObservableCollection<ChapterReadingProgress>(chapterProgress);
            }
        }
    }
}
