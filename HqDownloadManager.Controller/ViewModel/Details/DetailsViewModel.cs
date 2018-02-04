using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.ViewModel.Details {
    public class DetailsViewModel : ViewModelBase {
        private Hq _hq;
        private Chapter _selectedChapter;

        public Chapter SelectedChapter {
            get => _selectedChapter;
            set { _selectedChapter = value;
                OnPropertyChanged("SelectedChapter");
            }
        }


        public Hq Hq {
            get => _hq;
            set { _hq = value;
                OnPropertyChanged("Hq");
            }
        }

    }
}
