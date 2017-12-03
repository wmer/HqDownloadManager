using HqDownloadManager.Controller.Models;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.ViewModel.Follows {
    public class FollowUpdatesViewModel : ViewModelBase{
        private ObservableCollection<FollowItem> _updates;

        public ObservableCollection<FollowItem> Updates {
            get => _updates;
            set { _updates = value;
                OnPropertyChanged("Updates");
            }
        }

    }
}
