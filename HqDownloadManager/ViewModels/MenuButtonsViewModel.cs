using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.ViewModels {
    public class MenuButtonsViewModel : ViewModelBase {
        private bool _isHqUpdates;
        private bool _isAllHqs;
        private bool _isMyLibrary;
        private bool _isDownloadPage;
        private bool _isSettings;
        private bool _isReadingHistory;
        private bool _isDownloadHistory;

        public bool IsDownloadHistory {
            get { return _isDownloadHistory; }
            set { _isDownloadHistory = value;
                OnPropertyChanged("IsDownloadHistory");
            }
        }


        public bool IsReadingHistory {
            get { return _isReadingHistory; }
            set { _isReadingHistory = value;
                OnPropertyChanged("IsReadingHistory");
            }
        }


        public bool IsHqUpdates {
            get {
                return _isHqUpdates;
            }
            set {
                _isHqUpdates = value;
                OnPropertyChanged("IsHqUpdates");
            }
        }
        public bool IsAllHqs {
            get {
                return _isAllHqs;
            }
            set {
                _isAllHqs = value;
                OnPropertyChanged("IsAllHqs");
            }
        }
        public bool IsMyLibrary {
            get {
                return _isMyLibrary;
            }
            set {
                _isMyLibrary = value;
                OnPropertyChanged("IsMyLibrary");
            }
        }

        public bool IsDownloadPage {
            get {
                return _isDownloadPage;
            }
            set {
                _isDownloadPage = value;
                OnPropertyChanged("IsDownloadPage");
            }
        }

        public bool IsSettings {
            get {
                return _isSettings;
            }
            set {
                _isSettings = value;
                OnPropertyChanged("IsSettings");
            }
        }
    }
}
