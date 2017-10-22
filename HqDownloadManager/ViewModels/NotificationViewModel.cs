using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HqDownloadManager.ViewModels {
    public class NotificationViewModel : ViewModelBase {
        private Visibility _visibility;
        private string _message;

        public string Message {
            get { return _message; }
            set { _message = value;
                OnPropertyChanged("Message");
            }
        }


        public Visibility Visibility {
            get { return _visibility; }
            set { _visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

    }
}
