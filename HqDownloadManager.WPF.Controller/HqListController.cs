using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqDownloadManager.Shared.Database;
using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.List;
using HqManager.Models;
using SqlCreator.Enumerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Controller {
    public class HqListController : System.Windows.Controls.Page {
        private UserContext _userContext;
        private ConfigurationContext _configurationContext;
        protected DownloadManager _downloadManager;
        private HqListViewModel _hqListViewModel;

        public HqListController(UserContext userContext, ConfigurationContext configurationContext, DownloadManager downloadManager) {
            _userContext = userContext;
            _configurationContext = configurationContext;
            _downloadManager = downloadManager;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _hqListViewModel = ControlsHelper.FindResource<HqListViewModel>("MyList");
            _hqListViewModel.Entries = new ObservableCollection<HqEntry>(_userContext.HqEntry.FindAll());
            _hqListViewModel.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedEntry") {
                _hqListViewModel.ShowDetails = true;
                var hq = _hqListViewModel.Entry.Hq;
                if (hq != null && hq.Id > 0) {
                    _hqListViewModel.Updates = _userContext.Chapter.Find().Where(x => x.Hq == hq && x.IsUpdate == true).Execute();
                }
            }
        }
        
        public void SaveEntry() {
            var status = _hqListViewModel.SelectedEntry;
            _userContext.HqEntry.Update(status);
        }

        public void DownloadUpdates() {
            var hq =_hqListViewModel.Entry.Hq;
            var downloadLocation = SetDefaultPath();
            if (!string.IsNullOrEmpty(downloadLocation) && hq != null && hq.Id > 0) {
                if (!(_configurationContext.DownloadLocation.Find().Where(x => x.Location == downloadLocation).Execute().FirstOrDefault() is DownloadLocation)) {
                    _configurationContext.DownloadLocation.Save(new DownloadLocation { Location = downloadLocation }, Conflict.REPLACE);
                }
                hq.Chapters = _hqListViewModel.Updates;
                _downloadManager.AddToDownloadList(hq, downloadLocation);
            }
        }

        public void AddSelectedsToDownload() {
            var listView = ControlsHelper.Find<System.Windows.Controls.ListView>("UpdateList");
            var hq = _hqListViewModel.Entry.Hq;
            hq.Chapters = new List<Chapter>();
            foreach (var item in listView.SelectedItems) {
                hq.Chapters.Add(item as Chapter);
            }
            
            var downloadLocation = SetDefaultPath();
            if (!string.IsNullOrEmpty(downloadLocation)) {
                _configurationContext.DownloadLocation.Save(new DownloadLocation { Location = downloadLocation });
                _downloadManager.AddToDownloadList(hq, downloadLocation);
            }
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
    }
}
