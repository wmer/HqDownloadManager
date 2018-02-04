using HqDownloadManager.Controller.CustomEvents;
using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Controller.ViewModel.HqStatus;
using HqDownloadManager.Controller.ViewModel.MyDownloads;
using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Managers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Sources;
using HqDownloadManager.Download;
using HqDownloadManager.Download.Models;
using HqManager;
using HqManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controller {
    public class DownloadDetailsController : Windows.UI.Xaml.Controls.Page {
        protected ControlsHelper _controllerHelper;
        protected DownloadManager _downloadManager;
        private SourceManager _sourceManager;
        protected EntryManager _entryManager;
        private readonly NavigationHelper _navigationHelper;
        protected DownloadDetailsViewModel _detailsViewModel;
        protected NotificationViewModel _notification;
        protected EditDownloadViewModel _editModel;
        protected HqStatusViewModel _hqStatus;

        public DownloadDetailsController(
            ControlsHelper controllerHelper,
             DownloadManager downloadManager,
             SourceManager sourceManager,
             EntryManager entryManager,
             NavigationHelper navigationHelper) {
            _controllerHelper = controllerHelper;
            _downloadManager = downloadManager;
            _sourceManager = sourceManager;
            _entryManager = entryManager;
            _navigationHelper = navigationHelper;
            NavigationEventHub.Navigated += NavigationEventHub_Navigated;
            CoreEventHub.ProcessingProgress += CoreEventHub_ProcessingProgress;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _notification = _controllerHelper.FindResource<NotificationViewModel>("Notification");
            _editModel = _controllerHelper.FindResource<EditDownloadViewModel>("Edit");
            _hqStatus = _controllerHelper.FindResource<HqStatusViewModel>("HqStatus");
            _hqStatus.PropertyChanged += HqStatusChanged;

        }

        public void OpenEditor() {
            _editModel.Hq = _detailsViewModel.DownloadInfo.Hq;
            _editModel.Visibility = Visibility.Visible;
        }

        public void SearchDetails() {
            Task.Run(async () => {
                var result = new List<Hq>();
                var link = "";
                var title = "";
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _notification.Visibility = Visibility.Visible;
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
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        _editModel.Results = result;
                        _editModel.ResultVisibility = Visibility.Visible;
                    });
                } else {
                    var source = _sourceManager.GetSpurce(SourcesEnum.MangaHost) as MangaHostSourceManager;
                    source.Search(title, out result);
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        _editModel.Results = result;
                        _editModel.ResultVisibility = Visibility.Visible;
                    });
                }
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _notification.Visibility = Visibility.Collapsed;
                });
            });
        }

        public void SaveDetails() {
            Task.Run(async () => {
                var info = new HqDownloadInfo();
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    info = _detailsViewModel.DownloadInfo;
                    info.Hq = _editModel.Hq;
                    info.Hq.Chapters = _detailsViewModel.DownloadInfo.Hq.Chapters;
                });
                _downloadManager.SaveDownloadInfo(info);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _detailsViewModel.DownloadInfo = info;
                    _hqStatus.Hq = info.Hq;
                    _editModel.Visibility = Visibility.Collapsed;
                    _editModel.ResultVisibility = Visibility.Collapsed;
                });
            });
        }

        public void SaveEntry() {
            var entry = _hqStatus.Entry;
            _entryManager.SaveEntry(_hqStatus.Entry);
            _hqStatus.Visibility = Visibility.Collapsed;
        }

        public void DeleteHq() {
            _downloadManager.DeleteDownloadInfo(_detailsViewModel.DownloadInfo, true);
            NavigationHelper.GoBack();
        }

        public void SearchUpdates() {
            Task.Run(async () => {
                var link = "";
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _detailsViewModel.UpdateVisibility = Visibility.Visible;
                    _notification.Visibility = Visibility.Visible;
                    link = _detailsViewModel.DownloadInfo.Hq.Link;
                });
                if (!string.IsNullOrEmpty(link)) {
                    var source = _sourceManager.GetSourceFromLink(link);
                    var hq = new Hq();
                    source.GetInfo(link, out hq);
                    if (hq != null && !string.IsNullOrEmpty(hq.Link)) {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                            _detailsViewModel.UpdateList = hq.Chapters;
                        });
                    }
                }

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _notification.Visibility = Visibility.Collapsed;
                });
            });
        }

        public void AddSelectedsToDownload() {
            var listView = _controllerHelper.Find<ListView>("Updates");
            var hq = _detailsViewModel.DownloadInfo.Hq;
            hq.Chapters = new List<Chapter>();
            foreach (var item in listView.SelectedItems) {
                hq.Chapters.Add(item as Chapter);
            }
            
            _downloadManager.AddToDownloadList(hq, $"{CoreConfiguration.DownloadLocation}\\Hqs");
            _detailsViewModel.UpdateVisibility = Visibility.Collapsed;
        }

        public virtual void HqStatusChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (_hqStatus.Hq != null && !string.IsNullOrEmpty(_hqStatus.Hq.Link) && e.PropertyName == "SelectedStatus" && _hqStatus.SelectedStatus != "Add to...") {
                _hqStatus.Visibility = Visibility.Visible;
            }
        }

        public void Read<T>(Chapter clickedChapter) where T : Windows.UI.Xaml.Controls.Page {
            var gridView = _controllerHelper.Find<GridView>("DownloadInfoChapters");
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

            _navigationHelper.Navigate<T>("Reader", readerModel);
        }

        private void NavigationEventHub_Navigated(object sender, NavigationEventArgs e) {
            _detailsViewModel = _controllerHelper.FindResource<DownloadDetailsViewModel>("Details");
            if (_detailsViewModel != null && e.ExtraContent != null) {
                _detailsViewModel.DownloadInfo = e.ExtraContent as HqDownloadInfo;
                _hqStatus.Entry = new HqEntry { ReadStatus = "Add to..." };
                if (!string.IsNullOrEmpty(_detailsViewModel.DownloadInfo.Hq.Link)) {
                    _hqStatus.Entry = _entryManager.GetHqEntry(_detailsViewModel.DownloadInfo.Hq);
                    _hqStatus.Hq = _detailsViewModel.DownloadInfo.Hq;
                }
            }
        }

        private void CoreEventHub_ProcessingProgress(object sender, ProcessingEventArgs ev) {
            Task.Run(async () => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    if (_notification != null) {
                        _notification.Message = ev.StateMessage;
                    }
                });
            });
        }
    }
}
