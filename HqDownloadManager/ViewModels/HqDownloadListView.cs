using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Models;

namespace HqDownloadManager.ViewModels {
    public class HqDownloadListView : ViewModelBase {
        private ObservableCollection<DownloadListItem> _hqs;

        public HqDownloadListView() {
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
