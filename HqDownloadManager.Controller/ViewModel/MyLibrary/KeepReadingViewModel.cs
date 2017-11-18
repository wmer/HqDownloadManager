using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.ViewModel.MyLibrary {
    public class KeepReadingViewModel : ViewModelBase {
        private ObservableCollection<ReaderViewModel> _readings;
        private ReaderViewModel _selectedItem;
        private DateTime _date;

        public KeepReadingViewModel() {
            Readings = new ObservableCollection<ReaderViewModel>();
            Readings.CollectionChanged += Readings_CollectionChanged;
        }

        public DateTime Date {
            get => _date;
            set {
                _date = value;
                OnPropertyChanged("Date");
            }
        }


        public ObservableCollection<ReaderViewModel> Readings {
            get => _readings;
            set {
                _readings = value;
                OnPropertyChanged("Readings");
            }
        }

        public ReaderViewModel SelectedItem {
            get => _selectedItem;
            set {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        private void Readings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged("Readings");
        }
    }
}
