using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.ViewModels {
    public class LogViewModel : ViewModelBase {
        private string _log;

        public string Log {
            get => _log;
            set {
                _log = value;
                OnPropertyChanged("Log");
            }
        }

    }
}
