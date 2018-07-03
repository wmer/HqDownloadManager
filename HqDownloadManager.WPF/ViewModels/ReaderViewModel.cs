using HqDownloadManager.Helpers;
using HqDownloadManager.Models;
using HqDownloadManager.WPF.Commands;
using HqDownloadManager.WPF.Databases;
using HqDownloadManager.WPF.Models;
using MangaScraping;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;
using WPF.Tools.Navigation.Events;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.ViewModels {
    public class ReaderViewModel : ViewModelBase {
        private readonly SourceManager _sourceManager;

        private readonly DownloadHelper _downloadHelper;
        private readonly ReaderContext _readerContext;
        private ChangePageCommand _changePageCommand;
        private ShortcutsReaderCommand _shortcutsReaderCommand;
        private ScrollHorizontallyCommand _scrollHorizontallyCommand;

        private DownloadedHq _downloadedHq;

        public DownloadedHq DownloadedHq {
            get { return _downloadedHq; }
            set {
                _downloadedHq = value;
                OnPropertyChanged("DownloadedHq");
            }
        }

        private DownloadedChapter _previousChapter;

        public DownloadedChapter PreviousChapter {
            get { return _previousChapter; }
            set {
                _previousChapter = value;
                OnPropertyChanged("PreviousChapter");
            }
        }

        private DownloadedChapter _actualChapter;

        public DownloadedChapter ActualChapter {
            get { return _actualChapter; }
            set {
                _actualChapter = value;
                OnPropertyChanged("ActualChapter");
                LoadPages();
            }
        }

        private int _actualChapterIndex;

        public int ActualChapterIndex {
            get { return _actualChapterIndex; }
            set {
                _actualChapterIndex = value;
                OnPropertyChanged("ActualChapterIndex");
            }
        }


        private DownloadedChapter _nextChapter;

        public DownloadedChapter NextChapter {
            get { return _nextChapter; }
            set {
                _nextChapter = value;
                OnPropertyChanged("NextChapter");
            }
        }

        private MangaScraping.Models.Page _actualPage;

        public MangaScraping.Models.Page ActualPage {
            get { return _actualPage; }
            set {
                _actualPage = value;
                OnPropertyChanged("ActualPage");
            }
        }

        private int _actualPageIndex;

        public int ActualPageIndex {
            get { return _actualPageIndex; }
            set {
                _actualPageIndex = value;
                OnPropertyChanged("ActualPageIndex");
            }
        }

        private bool _detailsVisibility;

        public bool DetailsVisibility {
            get { return _detailsVisibility; }
            set {
                _detailsVisibility = value;
                OnPropertyChanged("DetailsVisibility");
            }
        }


        private bool _controlsVisibility;

        public bool ControlsVisibility {
            get { return _controlsVisibility; }
            set {
                _controlsVisibility = value;
                OnPropertyChanged("ControlsVisibility");
            }
        }

        public ReaderViewModel(
                        DownloadHelper downloadHelper,
                        ReaderContext readerContext,
                        ChangePageCommand changePageCommand,
                        ShortcutsReaderCommand shortcutsReaderCommand,
                        ScrollHorizontallyCommand scrollHorizontallyCommand,
                        SourceManager sourceManager) {
            _downloadHelper = downloadHelper;
            _readerContext = readerContext;
            _changePageCommand = changePageCommand;
            _shortcutsReaderCommand = shortcutsReaderCommand;
            _scrollHorizontallyCommand = scrollHorizontallyCommand;
            _sourceManager = sourceManager;

            ActualPageIndex = -1;
            NavigationEventHub.Navigated += OnNavigated;
        }

        public DelegateCommand<Tuple<ScrollChangedEventArgs, ListView, ReaderViewModel>> PageChanged {
            get => _changePageCommand.Command;
        }

        public DelegateCommand<ReaderViewModel> Shortcuts {
            get => _shortcutsReaderCommand.Command;
        }

        public DelegateCommand<MouseWheelEventArgs> ScrollHorizontally {
            get => _scrollHorizontallyCommand.Command;
        }

        private void OnNavigated(object sender, NavigationEventArgs e) {
            Task.Run(() => {
                if (e.ExtraContent != null &&
                        e.ExtraContent.Count() == 2 &&
                        e.ExtraContent[0] is DownloadedHq downloaded &&
                        e.ExtraContent[1] is DownloadedChapter actualChapter) {
                    DownloadedHq = downloaded;
                    var index = DownloadedHq.Chapters.IndexOf(actualChapter);
                    var pages = _downloadHelper.GetPages(actualChapter.Location);
                    actualChapter.Chapter.Pages = pages;
                    ActualChapterIndex = index;
                    ActualChapter = actualChapter;
                }
            });
        }


        private void LoadPages() {
            if (ActualChapter != null &&
                ActualChapter.Chapter != null &&
                ActualChapter.Chapter.Pages == null ||
                ActualChapter.Chapter.Pages.Count() == 0) {
                var listView = ControlsHelper.Find<ListView>("FlipViewReader");
                Task.Run(() => {
                    var actualChapter = ActualChapter;
                    var pages = _downloadHelper.GetPages(actualChapter.Location);
                    if (pages == null || pages.Count() == 0) {
                        var hq = DownloadedHq.Hq;
                        var source = _sourceManager.GetSourceFromLink(hq.Link);
                        source.GetInfo(ActualChapter.Chapter.Link, out Chapter chap);
                        pages = chap.Pages;
                    }
                    actualChapter.Chapter.Pages = pages;
                    ActualChapter = actualChapter;
                    ActualPageIndex = 0;
                    ActualPage = pages[0];
                    listView.ScrollIntoView(ActualPage);
                    SaveReader();
                });
            }
        }

        private void SaveReader() {
            if (_readerContext.ChapterReadingProgress.Find()
                                                    .Where(x => x.Location == ActualChapter.Location)
                                                    .Execute()
                                                    .FirstOrDefault() is ChapterReadingProgress chapterReading) {
                chapterReading.Location = ActualChapter.Location;
                chapterReading.ChapterIndex = DownloadedHq.Chapters.IndexOf(ActualChapter);
                chapterReading.Date = DateTime.Now;
                chapterReading.ActualPage = ActualPageIndex;
                chapterReading.TotalPages = ActualChapter.Chapter.Pages.Count();
                chapterReading.LastPageLocation = ActualPage.Source;
                chapterReading.ChapterTitle = ActualChapter.Chapter.Title;

                _readerContext.ChapterReadingProgress.Update(chapterReading);
            } else {
                var chapReading = new ChapterReadingProgress {
                    ActualPage = ActualPageIndex,
                    ChapterIndex = DownloadedHq.Chapters.IndexOf(ActualChapter),
                    Date = DateTime.Now,
                    HqLocation = DownloadedHq.Location,
                    Location = ActualChapter.Location,
                    TotalPages = ActualChapter.Chapter.Pages.Count(),
                    LastPageLocation = ActualPage.Source,
                    ChapterTitle = ActualChapter.Chapter.Title
                };

                _readerContext.ChapterReadingProgress.Save(chapReading);
            }
        }
    }
}
