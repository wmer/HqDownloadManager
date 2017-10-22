using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.ViewModels {
    public class HqListViewModel : ViewModelBase {
        private ObservableCollection<Hq> hqs;

        public HqListViewModel() {
            hqs = new ObservableCollection<Hq>();
            hqs.CollectionChanged += Hqs_CollectionChanged;
        }

        public ObservableCollection<Hq> Hqs {
            get { return hqs; }
            set {
                hqs = value;
                OnPropertyChanged("hqs");
            }
        }

        private void Hqs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged("hqs");
        }
    }
}
