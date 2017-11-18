using HqDownloadManager.Controller.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.ViewModel.DownloadPage {
    public class DownloadListViewModel : ViewModelBase {
        private ObservableCollection<DownloadListItem> _hqs;

        public DownloadListViewModel() {
            _hqs = new ObservableCollection<DownloadListItem>();
            _hqs.CollectionChanged += Hqs_CollectionChanged;
        }

        public ObservableCollection<DownloadListItem> Hqs {
            get => _hqs;
            set {
                _hqs = value;
                OnPropertyChanged("Hqs");
            }
        }

        private void Hqs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged("Hqs");
        }

    }
}
