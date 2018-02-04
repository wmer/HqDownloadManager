using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.ViewModel.SourceLibrary {
    public class SourceLibraryViewModel : ViewModelBase {
        private ObservableCollection<Hq> _hqLibrary;
        private Dictionary<string, string> _lethers;
        private Hq _selectedHq;
        private double _width;
        private double _height;

        public Hq SelectedHq {
            get => _selectedHq;
            set { _selectedHq = value;
                OnPropertyChanged("SelectedHq");
            }
        }

        public ObservableCollection<Hq> HqLibrary {
            get => _hqLibrary;
            set {
                _hqLibrary = value;
                OnPropertyChanged("HqLibrary");
            }
        }
        public Dictionary<string, string> Lethers {
            get => _lethers;
            set {
                _lethers = value;
                OnPropertyChanged("Lethers");
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
