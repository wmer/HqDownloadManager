using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.ViewModel.MyLibrary {
    public class DownloadDetailsViewModel : ViewModelBase {
        private Hq _hq;
        private ObservableCollection<ReaderViewModel> _readerViewModel;
        private List<Chapter> _updates;
        private ReaderViewModel _selectedChapter;
        private double _width;
        private double _height;


        public Hq Hq {
            get => _hq;
            set { _hq = value;
                OnPropertyChanged("Hq");
            }
        }

        public ObservableCollection<ReaderViewModel> ReaderViewModel {
            get => _readerViewModel;
            set {
                _readerViewModel = value;
                OnPropertyChanged("ReaderViewModel");
            }
        }

        public List<Chapter> Updates {
            get => _updates;
            set {
                _updates = value;
                OnPropertyChanged("Updates");
            }
        }

        public ReaderViewModel SelectedChapter {
            get => _selectedChapter;
            set {
                _selectedChapter = value;
                OnPropertyChanged("SelectedChapter");
            }
        }

        public double Width {
            get => _width;
            set {
                _width = value;
                OnPropertyChanged("Width");
            }
        }
        public double Height {
            get => _height;
            set {
                _height = value;
                OnPropertyChanged("Height");
            }
        }
    }
}
