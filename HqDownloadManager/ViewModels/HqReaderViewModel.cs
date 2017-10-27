using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.Models;

namespace HqDownloadManager.ViewModels {
    public class HqReaderViewModel : ViewModelBase {
        private Chapter _previousChapter;
        private Chapter _actualChapter;
        private Chapter _nextChapter;
        private int _actualPage;

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


        public Chapter PreviousChapter {
            get => _previousChapter;
            set {
                _previousChapter = value;
                OnPropertyChanged("PreviousChapter");
            }
        }

    }
}
