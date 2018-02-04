using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.ViewModel.SourceUpdate {
    public class SourceUpdateViewModel : ViewModelBase {
        private ObservableCollection<Update> _updates;
        private Update _selectedUpdate;
        private double _width;
        private double _height;

        public Update SelectedUpdate {
            get => _selectedUpdate;
            set { _selectedUpdate = value;
                OnPropertyChanged("SelectedUpdate");
            }
        }

        public ObservableCollection<Update> Updates {
            get => _updates;
            set { _updates = value;
                OnPropertyChanged("Updates");
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
