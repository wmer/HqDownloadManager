using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.Models;

namespace HqDownloadManager.ViewModels {
    public class HqDetailsViewModel : ViewModelBase {
        private Hq _hq;

        public Hq Hq {
            get => _hq;
            set {
                _hq = value;
                OnPropertyChanged("Hq");
            }
        }

    }
}
