using DependencyInjectionResolver;
using HqDownloadManager.Download;
using HqDownloadManager.Helpers;
using HqDownloadManager.Models;
using HqDownloadManager.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.ViewModel;

namespace HqDownloadManager.WPF.ViewModels {
    public class DownloadListViewModel : ViewModelBase {
        private StartDownloadCommand _startDownloadCommand;
        private DownloadManager _downloadManager;
        private ObservableCollection<DownloadItem> _downloadList;
        private DownloadItem _actualDownload;
        private string _message;

        public DownloadListViewModel(
                        StartDownloadCommand startDownloadCommand, 
                        DownloadManager downloadManager) {
            _startDownloadCommand = startDownloadCommand;
            _downloadManager = downloadManager;
            DownloadList = _downloadManager.GetDownloadList();
        }

        public string Message {
            get => _message;
            set {
                _message = value;
                OnPropertyChanged("Message");
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
                StartDownload.RaiseCanExecuteChanged();
            }
        }

        public StartDownloadCommand StartDownload {
            get { return _startDownloadCommand; }
        }
    }
}
