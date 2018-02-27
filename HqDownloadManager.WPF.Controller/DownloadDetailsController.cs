using HqDownloadManager.Core;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Managers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Sources;
using HqDownloadManager.Download;
using HqDownloadManager.Download.Configuration;
using HqDownloadManager.Download.Models;
using HqDownloadManager.Shared.Database;
using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.HqStatus;
using HqDownloadManager.Shared.ViewModel.MyDownloads;
using HqDownloadManager.Shared.ViewModel.Reader;
using HqDownloadManager.Shared.ViewModel.Shared;
using HqManager;
using HqManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using WPF.Tools.Navigation;
using WPF.Tools.Navigation.Events;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Controller {
    public class DownloadDetailsController : System.Windows.Controls.Page {
        protected DownloadManager _downloadManager;
        private SourceManager _sourceManager;
        protected EntryManager _entryManager;
        private UserContext _userContext;
        private NavigationManager _navigationManager;
        protected DownloadDetailsViewModel _detailsViewModel;
        protected NotificationViewModel _notification;
        protected EditDownloadViewModel _editModel;
        protected HqStatusViewModel _hqStatus;
        private System.Windows.Controls.ListView _list;

        public DownloadDetailsController(
             DownloadManager downloadManager,
             SourceManager sourceManager,
             EntryManager entryManager,
             UserContext userContext,
             NavigationManager navigationManager) {
            _downloadManager = downloadManager;
            _sourceManager = sourceManager;
            _entryManager = entryManager;
            _userContext = userContext;
            _navigationManager = navigationManager;
            NavigationEventHub.Navigated += NavigationEventHub_Navigated;
            CoreEventHub.ProcessingProgress += CoreEventHub_ProcessingProgress;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _list = ControlsHelper.Find<System.Windows.Controls.ListView>("DownloadInfoChapters");
            _notification = ControlsHelper.FindResource<NotificationViewModel>("Notification");
            _editModel = ControlsHelper.FindResource<EditDownloadViewModel>("Edit");
            _hqStatus = ControlsHelper.FindResource<HqStatusViewModel>("HqStatus");
            _hqStatus.PropertyChanged += HqStatusChanged;

        }

        public void OpenEditor() {
            _editModel.Hq = _detailsViewModel.DownloadInfo.Hq;
            _editModel.Visibility = true;
            SearchDetails();
        }

        public void SearchDetails() {
            Task.Run(() => {
                var result = new List<Hq>();
                var link = "";
                var title = "";
                Dispatcher.Invoke(() => {
                    _notification.Visibility = true;
                    link = _editModel.Hq.Link;
                    title = _editModel.Hq.Title;

                });
                if (!string.IsNullOrEmpty(link)) {
                    var source = _sourceManager.GetSourceFromLink(link);
                    var hq = new Hq();
                    source.GetInfo(link, out hq);
                    if (hq != null && !string.IsNullOrEmpty(hq.Link)) {
                        result.Add(hq);
                        _entryManager.AddTo(ReadStatus.PRETENDO_LER, hq);
                    }
                    Dispatcher.Invoke(() => {
                        _editModel.Results = result;
                        _editModel.ResultVisibility = true;
                    });
                } else if (!string.IsNullOrEmpty(title)) {
                    var source = _sourceManager.GetSpurce(SourcesEnum.MangaHost) as MangaHostSourceManager;
                    source.Search(title, out result);
                    Dispatcher.Invoke(() => {
                        _editModel.Results = result;
                        _editModel.ResultVisibility = true;
                    });
                }
                Dispatcher.Invoke(() => {
                    _notification.Visibility = false;
                });
            });
        }

        public void SaveDetails() {
            Task.Run(() => {
                var info = new HqDownloadInfo();
                Dispatcher.Invoke(() => {
                    info = _detailsViewModel.DownloadInfo;
                    info.Hq = _editModel.Hq;
                    info.Hq.Chapters = _detailsViewModel.DownloadInfo.Hq.Chapters;
                });
                _downloadManager.SaveDownloadInfo(info);
                Dispatcher.Invoke(() => {
                    _detailsViewModel.DownloadInfo = info;
                    _hqStatus.Hq = info.Hq;
                    _editModel.Visibility = false;
                    _editModel.ResultVisibility = false;
                });
            });
        }

        public void SaveEntry() {
            var entry = _hqStatus.Entry;
            _entryManager.SaveEntry(_hqStatus.Entry);
            _hqStatus.Visibility = false;
        }

        public void DeleteHq() {
            var id = _detailsViewModel.DownloadInfo.Hq.Id;
            var path = _detailsViewModel.DownloadInfo.Path;
            var hq = new Hq { Id = id };
            _hqStatus.Hq = null;
            _detailsViewModel.DownloadInfo = null;
            _list = null;
            var info = new HqDownloadInfo { Hq = hq, Path = path };
            Task.Run(() => {
                _downloadManager.DeleteDownloadInfo(info, true);
                Dispatcher.Invoke(() => {
                    NavigationManager.GoBack();
                });
            });
        }

        public void SearchUpdates() {
            Task.Run(() => {
                var link = "";
                Dispatcher.Invoke(() => {
                    _detailsViewModel.UpdateVisibility = true;
                    _notification.Visibility = true;
                    link = _detailsViewModel.DownloadInfo.Hq.Link;
                });
                if (!string.IsNullOrEmpty(link)) {
                    var source = _sourceManager.GetSourceFromLink(link);
                    var hq = new Hq();
                    source.GetInfo(link, out hq);
                    if (hq != null && !string.IsNullOrEmpty(hq.Link)) {
                        Dispatcher.Invoke(() => {
                            _detailsViewModel.UpdateList = hq.Chapters;
                        });
                    }
                }

                Dispatcher.Invoke(() => {
                    _notification.Visibility = false;
                });
            });
        }

        public void AddSelectedsToDownload() {
            var listView = ControlsHelper.Find<System.Windows.Controls.ListView>("Updates");
            var hq = _detailsViewModel.DownloadInfo.Hq;
            hq.Chapters = new List<Chapter>();
            foreach (var item in listView.SelectedItems) {
                hq.Chapters.Add(item as Chapter);
            }

            var downloadLocation = Directory.GetParent(_detailsViewModel.DownloadInfo.Path).FullName;
            if (!string.IsNullOrEmpty(downloadLocation)) {
                _downloadManager.AddToDownloadList(hq, downloadLocation);
            }
            _detailsViewModel.UpdateVisibility = false;
        }

        public virtual void HqStatusChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (_hqStatus.Hq != null && !string.IsNullOrEmpty(_hqStatus.Hq.Link) && e.PropertyName == "SelectedStatus" && _hqStatus.SelectedStatus != "Add to...") {
                _hqStatus.Visibility = true;
            }
        }

        public void Read<T>(Chapter clickedChapter) where T : System.Windows.Controls.Page {
            var gridView = ControlsHelper.Find<System.Windows.Controls.ListView>("DownloadInfoChapters");
            _detailsViewModel.SelectedChapter = clickedChapter;
            var readerModel = new ReaderViewModel {
                Hq = _detailsViewModel.DownloadInfo.Hq,
                ActualChapter = _detailsViewModel.SelectedChapter,
                ActualChapterIndex = gridView.SelectedIndex
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

        public void Read<T>(ReaderHistory history) where T : System.Windows.Controls.Page => _navigationManager.Navigate<T>("Reader", history.Reader);

        private void NavigationEventHub_Navigated(object sender, NavigationEventArgs e) {
            _detailsViewModel = ControlsHelper.FindResource<DownloadDetailsViewModel>("Details");
            if (_detailsViewModel != null && e.ExtraContent is HqDownloadInfo extraContent) {
                _detailsViewModel.DownloadInfo = extraContent;
                _hqStatus.Entry = new HqEntry { ReadStatus = "Add to..." };
                if (_detailsViewModel.DownloadInfo.Hq != null && !string.IsNullOrEmpty(_detailsViewModel.DownloadInfo.Hq.Link)) {
                    _hqStatus.Entry = _entryManager.GetHqEntry(_detailsViewModel.DownloadInfo.Hq);
                    _hqStatus.Hq = _detailsViewModel.DownloadInfo.Hq;
                    if (_userContext.ReaderHistory.Find().Where(x => x.Link == _hqStatus.Hq.Link).Execute() is List<ReaderHistory> list) {
                        var listM = new List<ReaderHistory>();
                        foreach (var item in list) {
                            item.Reader.Hq = _detailsViewModel.DownloadInfo.Hq;
                            item.Reader.ActualChapter = _hqStatus.Hq.Chapters[item.Reader.ActualChapterIndex];
                            listM.Add(item);
                        }
                        _detailsViewModel.Readings = listM.Reverse<ReaderHistory>().ToList();
                    }
                }

            }
        }


        public int ChangeNumCollumns() {
            var grid = ControlsHelper.Find<Grid>("Principal");
            return Convert.ToInt32(grid.ActualWidth / 200);
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
