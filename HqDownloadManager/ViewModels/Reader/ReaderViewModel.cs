using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace HqDownloadManager.ViewModels.Reader {
    public class ReaderViewModel : ViewModelBase {
        private Hq _hq;
        private Chapter _previousChapter;
        private Chapter _actualChapter;
        private Chapter _nextChapter;
        private int _actualChapterIndex;
        private int _actualPage;

        private double _pageWidth;
        private double _pageHeight;
        private double _progrssBarWidth;
        private Visibility _controlsVisibility;

        public Hq Hq {
            get => _hq;
            set {
                _hq = value;
                OnPropertyChanged("Hq");
            }
        }

        public int ActualPage {
            get => _actualPage;
            set {
                _actualPage = value;
                OnPropertyChanged("ActualPage");
            }
        }


        public Chapter NextChapter {
            get => _nextChapter;
            set {
                _nextChapter = value;
                OnPropertyChanged("NextChapter");
            }
        }


        public Chapter ActualChapter {
            get => _actualChapter;
            set {
                _actualChapter = value;
                OnPropertyChanged("ActualChapter");
            }
        }

        public int ActualChapterIndex {
            get => _actualChapterIndex;
            set {
                _actualChapterIndex = value;
                OnPropertyChanged("ActualChapterIndex");
            }
        }

        public Chapter PreviousChapter {
            get => _previousChapter;
            set {
                _previousChapter = value;
                OnPropertyChanged("PreviousChapter");
            }
        }

        public Visibility ControlsVisibility {
            get => _controlsVisibility;
            set {
                _controlsVisibility = value;
                OnPropertyChanged("ControlsVisibility");
            }
        }


        public double ProgressBarWidth {
            get => _progrssBarWidth;
            set {
                _progrssBarWidth = value;
                OnPropertyChanged("ProgressBarWidth");
            }
        }


        public double PageHeight {
            get => _pageHeight;
            set {
                _pageHeight = value;
                OnPropertyChanged("PageHeight");
            }
        }


        public double PageWidth {
            get => _pageWidth;
            set {
                _pageWidth = value;
                OnPropertyChanged("PageWidth");
            }
        }
    }
}
