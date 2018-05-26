using HqDownloadManager.WPF.Commands;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;

namespace HqDownloadManager.WPF.ViewModels {
    public class DetailsViewModel : ViewModelBase {
        private CloseDetailsCommand _closeDetailsCommand;
        private Hq _hq;
        private Chapter _selectedChapter;
        private int _selectedIndex;
        private bool _opened;
        
        public DetailsViewModel(CloseDetailsCommand closeDetailsCommand) {
            _closeDetailsCommand = closeDetailsCommand;
            SelectedIndex = -1;
        }

        public bool Opened {
            get => _opened;
            set {
                _opened = value;
                OnPropertyChanged("Opened");
                CloseDetails.RaiseCanExecuteChanged();
            }
        }

        public Chapter SelectedChapter {
            get => _selectedChapter;
            set {
                _selectedChapter = value;
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
            set {
                _hq = value;
                OnPropertyChanged("Hq");
            }
        }

        public CloseDetailsCommand CloseDetails {
            get { return _closeDetailsCommand; }
        }
    }
}
