using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.ViewModel.DownloadPage {
    public class DownloadPageViewModel : ViewModelBase {
        private int _actualPage;
        private int _totalPages;

        public int TotalPages {
            get => _totalPages;
            set { _totalPages = value;
                OnPropertyChanged("TotalPages");
            }
        }

        private string _message;

        public string Message {
            get  => _message;
            set { _message = value;
                OnPropertyChanged("Message");
            }
        }


        public int ActualPage {
            get => _actualPage;
            set {
                _actualPage = value;
                OnPropertyChanged("ActualPage");
            }
        }
    }
}
