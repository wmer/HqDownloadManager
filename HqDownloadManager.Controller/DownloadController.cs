using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Controller.ViewModel.Download;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download;
using HqDownloadManager.Download.Models;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using HqDownloadManager.Download.CustomEventArgs;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls.Primitives;

namespace HqDownloadManager.Controller {
    public class DownloadController : Windows.UI.Xaml.Controls.Page {
        protected ControlsHelper _controllerHelper;
        protected DownloadManager _downloadManager;
        protected DownloadListViewModel _downloadListViewModel;
        protected ListView _downloadList;
        protected ListView _chapterList;
        protected List<ProgressBar> _progressBars;

        public DownloadController(
                ControlsHelper controllerHelper,
                DownloadManager downloadManager) {
            _controllerHelper = controllerHelper;
            _downloadManager = downloadManager;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _downloadListViewModel = _controllerHelper.FindResource<DownloadListViewModel>("DownloadListView");
            _downloadList = _controllerHelper.Find<ListView>("DownloadList");
            _chapterList = _controllerHelper.Find<ListView>("HqChapters");

            DownloadEventHub.DownloadStart += OnDownloadStart;
            DownloadEventHub.DownloadEnd += OnDonwloadEnd;
            DownloadEventHub.DownloadChapterStart += OnDownloadChapterStart;
            DownloadEventHub.DownloadProgress += OnDownloadProgress;

            Task.Run(async () => {
                var downloadList = _downloadManager.GetDownloadList();
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _downloadListViewModel.DownloadList = downloadList;
                });
            });
        }

        public void Download() => _downloadManager.Download();
        public void PauseResumeDownload() => _downloadManager.PauseResumeDownload();
        public void StopDownload() => _downloadManager.StopDownload();
        public void ExcludeItem() => _downloadManager.ExcludeFromDownloadList(_downloadListViewModel.ActualDownload);

        private void ShowMessage(string message) {
            var previousMessage = _downloadListViewModel.Message;
            if (string.IsNullOrEmpty(previousMessage)) {
                _downloadListViewModel.Message = $"{DateTime.Now} - {message}";
            }else {
                _downloadListViewModel.Message = $"{previousMessage}" +
                    $"{Environment.NewLine}{DateTime.Now} - {message}";
            }
        }

        private void OnDownloadChapterStart(object sender, ProgressEventArgs ev) {
            Task.Run(async () => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _chapterList.ScrollIntoView(ev.Item.LastDownloadedChapter);
                    _chapterList.SelectedItem = ev.Item.LastDownloadedChapter;
                    

                    _progressBars[0].Maximum = ev.Total;
                    _progressBars[0].Value = ev.NumAtual;
                    _progressBars[1].Value = 0;

                    _downloadList.UpdateLayout();
                });
            });
        }

        private void OnDownloadProgress(object sender, ProgressEventArgs ev) {
            Task.Run(async () => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    
                    _progressBars[1].Maximum = ev.Total;
                    _progressBars[1].Value = ev.NumAtual;

                    _downloadList.UpdateLayout();
                });
            });
        }

        private void OnDonwloadEnd(object sender, DownloadEventArgs ev) {
            Task.Run(async () => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {

                });
            });
        }

        private void OnDownloadStart(object sender, DownloadEventArgs ev) {
            Task.Run(async () => {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _downloadListViewModel.ActualDownload = ev.Item;
                    _downloadList.ScrollIntoView(ev.Item);
                    var template = _downloadList.ContainerFromItem(ev.Item);
                    _progressBars = _controllerHelper.FindIn<ProgressBar>(template);
                });
            });
        }
    }
}
