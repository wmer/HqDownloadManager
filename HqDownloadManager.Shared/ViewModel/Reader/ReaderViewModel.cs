using HqDownloadManager.Core.Models;
using Repository.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HqDownloadManager.Shared.ViewModel.Reader {
    public class ReaderViewModel : ViewModelBase {
        private Hq _hq;
        private Chapter _previousChapter;
        private Chapter _actualChapter;
        private Chapter _nextChapter;
        private int _actualChapterIndex;
        private int _actualPage;
        private bool _controlsVisibility;
        private bool _detailsVisibility;

        public ReaderViewModel() {
            ActualPage = -1;
            ActualChapterIndex = -1;
        }

        [PrimaryKey]
        public int Id { get; set; }

        public virtual Hq Hq {
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

        [OnlyInModel]
        public virtual Chapter NextChapter {
            get => _nextChapter;
            set {
                _nextChapter = value;
                OnPropertyChanged("NextChapter");
            }
        }


        [OnlyInModel]
        public virtual Chapter ActualChapter {
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

        [OnlyInModel]
        public virtual Chapter PreviousChapter {
            get => _previousChapter;
            set {
                _previousChapter = value;
                OnPropertyChanged("PreviousChapter");
            }
        }

        [OnlyInModel]
        public bool ControlsVisibility {
            get => _controlsVisibility;
            set {
                _controlsVisibility = value;
                OnPropertyChanged("ControlsVisibility");
            }
        }

        [OnlyInModel]
        public bool DetailsVisibility {
            get => _detailsVisibility;
            set {
                _detailsVisibility = value;
                OnPropertyChanged("DetailsVisibility");
            }
        }
    }
}
