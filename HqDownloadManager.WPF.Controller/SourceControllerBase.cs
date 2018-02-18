using HqDownloadManager.Core;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Managers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqDownloadManager.Shared.Database;
using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.ComboBox;
using HqDownloadManager.Shared.ViewModel.Details;
using HqDownloadManager.Shared.ViewModel.HqStatus;
using HqDownloadManager.Shared.ViewModel.Reader;
using HqDownloadManager.Shared.ViewModel.Shared;
using HqManager;
using HqManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using WPF.Tools.Navigation;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Controller {
    public class SourceControllerBase : System.Windows.Controls.Page {
        protected delegate IHqSourceManager LibraryOperation(out LibraryPage library, double timeCache = 4320);

        private SourceManager _sourceManager;
        private ConfigurationContext _configurationContext;
        protected EntryManager _entryManager;
        protected DownloadManager _downloadManager;
        protected NavigationManager _navigationManager;
        protected NotificationViewModel _notification;
        protected HqSourceLibrarySelectorViewModel _sourceSelector;
        protected DetailsViewModel _detailsViewModel;
        protected HqStatusViewModel _hqStatusView;
        protected Dictionary<string, IHqSourceManager> _sources;
        protected IHqSourceManager _actualSource;

        public SourceControllerBase(
            SourceManager sourceManager, 
            ConfigurationContext configurationContext,
            EntryManager entryManage,
            DownloadManager downloadManager, 
            NavigationManager navigationManager) {
            _sourceManager = sourceManager;
            _configurationContext = configurationContext;
            _entryManager = entryManage;
            _downloadManager = downloadManager;
            _navigationManager = navigationManager;
            CoreEventHub.ProcessingProgress += CoreEventHub_ProcessingProgress;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _notification = ControlsHelper.FindResource<NotificationViewModel>("Notification");
            _sourceSelector = ControlsHelper.FindResource<HqSourceLibrarySelectorViewModel>("SourceSelector");
            _detailsViewModel = ControlsHelper.FindResource<DetailsViewModel>("DetailsView");
            _hqStatusView = ControlsHelper.FindResource<HqStatusViewModel>("HqStatus");
            _hqStatusView.PropertyChanged += HqStatusChanged;
            _sources = _sourceManager.GetSources();
            _actualSource = _sources[_sourceSelector.SelectedSource];
            _sourceSelector.PropertyChanged += SelectedSourceChanged;
        }


        public virtual void HqStatusChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedStatus") {

            }
        }

        public virtual void OpenDetails(Hq hq) {
            _detailsViewModel.Opened = false;
            _hqStatusView.Visibility = false;
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    _notification.Visibility = true;
                });
                var hqDetails = new Hq();
                _actualSource.GetInfo(hq.Link, out hqDetails);
                Dispatcher.Invoke(() => {
                    _hqStatusView.Entry = _entryManager.GetHqEntry(hq);
                    _hqStatusView.Hq = hqDetails;
                    _detailsViewModel.Hq = hqDetails;
                    _detailsViewModel.Opened = true;
                    _notification.Visibility = false;
                });
            });
        }

        public virtual void AddToDownloadList() {
            var hq = _hqStatusView.Hq;
            _entryManager.AddTo(ReadStatus.PRETENDO_LER, hq);
            var downloadLocation = SetDefaultPath();
            if (!string.IsNullOrEmpty(downloadLocation)) {
                if (!(_configurationContext.DownloadLocation.Find().Where(x => x.Location == downloadLocation).Execute().FirstOrDefault() is DownloadLocation)) {
                    _configurationContext.DownloadLocation.SaveOrReplace(new DownloadLocation { Location = downloadLocation });
                }
                _downloadManager.AddToDownloadList(hq, downloadLocation);
            }
        }

        public virtual void AddSelectedsToDownload() {
            var listView = ControlsHelper.Find<System.Windows.Controls.ListView>("HqChapters");
            var hq = _hqStatusView.Hq;
            hq.Chapters = new List<Chapter>();
            foreach (var item in listView.SelectedItems) {
                hq.Chapters.Add(item as Chapter);
            }

            _entryManager.AddTo(ReadStatus.PRETENDO_LER, hq);
            var downloadLocation = SetDefaultPath();
            if (!string.IsNullOrEmpty(downloadLocation)) {
                _configurationContext.DownloadLocation.Save(new DownloadLocation { Location = downloadLocation });
                _downloadManager.AddToDownloadList(hq, downloadLocation);
            }
        }

        public virtual void SelectedSourceChanged(object sender, global::System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedSource") {
                _actualSource = _sources[_sourceSelector.SelectedSource];
            }
        }

        public int ChangeNumCollumns() {
            var grid = ControlsHelper.Find<Grid>("Principal");
            return Convert.ToInt32(grid.ActualWidth / 200);
        }

        public void Read<T>(Chapter chapter) where T : System.Windows.Controls.Page {
            var readerModel = new ReaderViewModel {
                Hq = _detailsViewModel.Hq,
                ActualChapter = chapter,
                ActualChapterIndex = _detailsViewModel.Hq.Chapters.IndexOf(chapter)
            };
            var next = readerModel.ActualChapterIndex + 1;
            if (next < readerModel.Hq.Chapters.Count) {
                readerModel.NextChapter = readerModel.Hq.Chapters[next];
            }
            if (readerModel.ActualChapterIndex > 0) {
                readerModel.PreviousChapter = readerModel.Hq.Chapters[readerModel.ActualChapterIndex - 1];
            }

            _navigationManager.Navigate<T>("Reader", readerModel);
        }



        [STAThread]
        public string SetDefaultPath() {
            var selectedPath = "";
            var selectFolderDialog = new FolderBrowserDialog();
            if (selectFolderDialog.ShowDialog() == DialogResult.OK) {
                selectedPath = selectFolderDialog.SelectedPath;
            }
            return selectedPath;
        }

        private void CoreEventHub_ProcessingProgress(object sender, ProcessingEventArgs ev) {
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    if (_notification != null) {
                        _notification.Message = ev.StateMessage;
                    }
                });
            });
        }
    }
}
