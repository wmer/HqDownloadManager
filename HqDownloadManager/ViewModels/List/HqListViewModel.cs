using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.Models;

namespace HqDownloadManager.ViewModels.List {
    public class HqListViewModel : ViewModelBase {
        private ObservableCollection<Hq> _hqs;
        private Dictionary<string, string> _lethers;
        private Hq _selectedItem;
        private double _width;
        private double _height;

        public Hq SelectedItem {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }


        public HqListViewModel() {
            _hqs = new ObservableCollection<Hq>();
            _hqs.CollectionChanged += Hqs_CollectionChanged;
        }

        public Dictionary<string, string> Lethers {
            get => _lethers;
            set {
                _lethers = value;
                OnPropertyChanged("Lethers");
            }
        }

        public ObservableCollection<Hq> Hqs {
            get => _hqs;
            set {
                _hqs = value;
                OnPropertyChanged("Hqs");
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

        private void Hqs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged("Hqs");
        }
    }
}
