using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.ViewModel.Details {
    public class DetailsViewModel : ViewModelBase {
        private Hq _hq;
        private Chapter _selectedChapter;
        private int _selectedIndex;
        private bool _opened;

        public DetailsViewModel() {
            SelectedIndex = -1;
        }

        public bool Opened {
            get => _opened;
            set { _opened = value;
                OnPropertyChanged("Opened");
            }
        }
        
        public Chapter SelectedChapter {
            get => _selectedChapter;
            set { _selectedChapter = value;
                OnPropertyChanged("SelectedChapter");
            }
        }

        public int SelectedIndex {
            get => _selectedIndex;
            set {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
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
