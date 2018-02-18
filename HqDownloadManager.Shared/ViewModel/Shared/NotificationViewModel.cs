using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HqDownloadManager.Shared.ViewModel.Shared {
    public class NotificationViewModel : ViewModelBase {
        private bool _visibility;
        private string _message;

        public string Message {
            get => _message;
            set {
                _message = value;
                OnPropertyChanged("Message");
            }
        }


        public bool Visibility {
            get => _visibility;
            set {
                _visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

    }
}
