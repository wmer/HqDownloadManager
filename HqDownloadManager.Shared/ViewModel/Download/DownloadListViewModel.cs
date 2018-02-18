using HqDownloadManager.Download.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.ViewModel.Download {
    public class DownloadListViewModel : ViewModelBase {
        private ObservableCollection<DownloadItem> _downloadList;
        private DownloadItem _actualDownload;
        private int _actualChapterIndex;
        private int _actualPageIndex;
        private string _message;

        public string Message {
            get => _message;
            set { _message = value;
                OnPropertyChanged("Message");
            }
        }


        public int ActualPageIndex {
            get => _actualPageIndex;
            set { _actualPageIndex = value;
                OnPropertyChanged("ActualPageIndex");
            }
        }


        public int ActualChapterIndex {
            get => _actualChapterIndex;
            set { _actualChapterIndex = value;
                OnPropertyChanged("ActualChapterIndex");
            }
        }


        public DownloadItem ActualDownload {
            get => _actualDownload;
            set {
                _actualDownload = value;
                OnPropertyChanged("ActualDownload");
            }
        }


        public ObservableCollection<DownloadItem> DownloadList {
            get => _downloadList;
            set {
                _downloadList = value;
                OnPropertyChanged("DownloadList");
            }
        }

    }
}
