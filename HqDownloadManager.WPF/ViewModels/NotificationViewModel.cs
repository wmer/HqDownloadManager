using MangaScraping.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.WPF.ViewModels {
    public class NotificationViewModel : ViewModelBase {
        private bool _visibility;
        private string _message;

        public NotificationViewModel() {
            CoreEventHub.ProcessingStart += CoreEventHub_ProcessingStart;
            CoreEventHub.ProcessingProgress += CoreEventHub_ProcessingProgress;
            CoreEventHub.ProcessingEnd += CoreEventHub_ProcessingEnd;
        }

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


        private void CoreEventHub_ProcessingStart(object sender, ProcessingEventArgs ev) {
            Task.Run(() => {
                Visibility = true;
            });
        }

        private void CoreEventHub_ProcessingProgress(object sender, ProcessingEventArgs ev) {
            Task.Run(() => {
                Visibility = true;
                Message = ev.StateMessage;
            });
        }

        private void CoreEventHub_ProcessingEnd(object sender, ProcessingEventArgs ev) {
            Task.Run(() => {
                Visibility = false;
            });
        }
    }
}
