using HqDownloadManager.Download;
using HqDownloadManager.Events;
using HqDownloadManager.Models;
using HqDownloadManager.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;

namespace HqDownloadManager.WPF.ViewModels {
    public class DownloadListViewModel : ViewModelBase {
        private DeleteFromDownloadListCommand _deleteFromDownloadListCommand;
        private StartDownloadCommand _startDownloadCommand;
        private DownloadManager _downloadManager;
        private ObservableCollection<DownloadItem> _downloadList;
        private DownloadItem _actualDownload;
        private string _message;

        public DownloadListViewModel(
                        StartDownloadCommand startDownloadCommand,
                        DownloadManager downloadManager,
                        DeleteFromDownloadListCommand deleteFromDownloadListCommand) {
            _startDownloadCommand = startDownloadCommand;
            _deleteFromDownloadListCommand = deleteFromDownloadListCommand;
            _downloadManager = downloadManager;
            DownloadList = _downloadManager.GetDownloadList();

            DownloadEventHub.DownloadStart += OnDownloadStart;
            DownloadEventHub.DownloadPause += OnDownloadPause;
            DownloadEventHub.DownloadResume += OnDownloadResume;
            DownloadEventHub.DownloadProgress += OnDownloadProgress;
            DownloadEventHub.DownloadError += OnDownloadError;
            DownloadEventHub.DownloadStop += OnDownloadStop;
            DownloadEventHub.DownloadEnd += OnDownloadEnd;
            DownloadEventHub.DownloadChapterStart += OnDownloadChapterStart;
            DownloadEventHub.DownloadChapterEnd += OnDownloadChapterEnd;
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
                _deleteFromDownloadListCommand.RaiseCanExecuteChanged();
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

        public DelegateCommand<DownloadItem> DeleteItem {
            get { return _deleteFromDownloadListCommand.Command; }
        }


        //Events
        private void OnDownloadChapterEnd(object sender, DownloadEventArgs ev) {

        }

        private void OnDownloadChapterStart(object sender, ProgressEventArgs ev) {

        }

        private void OnDownloadEnd(object sender, DownloadEventArgs ev) {

        }

        private void OnDownloadStop(object sender, DownloadEventArgs ev) {

        }

        private void OnDownloadError(object sender, DownloadErrorEventArgs ev) {

        }

        private void OnDownloadProgress(object sender, ProgressEventArgs ev) {

        }

        private void OnDownloadResume(object sender, ProgressEventArgs ev) {

        }

        private void OnDownloadPause(object sender, ProgressEventArgs ev) {

        }

        private void OnDownloadStart(object sender, DownloadEventArgs ev) {

        }
    }
}
