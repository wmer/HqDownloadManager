using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.Models;

namespace HqDownloadManager.ViewModels {
    public class HqDownloadingViewModel : ViewModelBase {
        private Hq _hq;
        private Chapter _chapter;
        private int _actualChapter;
        private int _actualPage;

        public int ActualPage {
            get => _actualPage;
            set {
                _actualPage = value;
                OnPropertyChanged("ActualPage");
            }
        }


        public int ActualChapter {
            get => _actualChapter;
            set {
                _actualChapter = value;
                OnPropertyChanged("ActualChapter");
            }
        }


        public Chapter Chapter {
            get => _chapter;
            set {
                _chapter = value;
                OnPropertyChanged("Chapter");
            }
        }


        public Hq Hq {
            get => _hq;
            set {
                _hq = value;
                OnPropertyChanged("Hq");
            }
        }

    }
}
