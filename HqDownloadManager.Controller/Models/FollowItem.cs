using HqDownloadManager.Controller.ViewModel;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.Models {
    public class FollowItem : ViewModelBase {
        private Hq _hq;
        private DateTime _date;

        private ObservableCollection<Chapter> _updates;

        public ObservableCollection<Chapter> Updates {
            get => _updates;
            set {
                _updates = value;
                OnPropertyChanged("Updates");
            }
        }

        public DateTime Date {
            get => _date;
            set {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public Hq Hq {
            get => _hq;
            set {
                _hq = value;
                OnPropertyChanged("Hq");
            }
        }
    }
}
