using HqDownloadManager.Download;
using HqDownloadManager.Download.CustomEventArgs;
using HqDownloadManager.Shared.ViewModel.Download;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Controller {
    public class DownloadController : System.Windows.Controls.Page {
        protected DownloadManager _downloadManager;
        protected DownloadListViewModel _downloadListViewModel;
        protected ListView _downloadList;
        protected ListView _chapterList;
        protected List<ProgressBar> _progressBars;
        protected TextBlock _progress;

        public DownloadController(
                DownloadManager downloadManager) {
            _downloadManager = downloadManager;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _downloadListViewModel = ControlsHelper.FindResource<DownloadListViewModel>("DownloadListView");
            _downloadList = ControlsHelper.Find<ListView>("DownloadList");
            _chapterList = ControlsHelper.Find<ListView>("HqChapters");

            DownloadEventHub.DownloadStart += OnDownloadStart;
            DownloadEventHub.DownloadEnd += OnDonwloadEnd;
            DownloadEventHub.DownloadChapterStart += OnDownloadChapterStart;
            DownloadEventHub.DownloadChapterEnd += OnDownloadChapterEnd; ;
            DownloadEventHub.DownloadProgress += OnDownloadProgress;

            Task.Run(() => {
                var downloadList = _downloadManager.GetDownloadList();
                Dispatcher.Invoke(() => {
                    _downloadListViewModel.DownloadList = downloadList;
                });
            });
        }

        public void Download() => _downloadManager.Download();
        public void PauseResumeDownload() => _downloadManager.PauseResumeDownload();
        public void StopDownload() => _downloadManager.StopDownload();
        public void ExcludeItem() => _downloadManager.ExcludeFromDownloadList(_downloadListViewModel.ActualDownload);

        private void Shutdown() {
            var check = ControlsHelper.Find<CheckBox>("ChkShudown");
            if (check.IsChecked == true) {
                Process.Start("shutdown", "/s /t 0");
            }
        }

        private void OnDownloadChapterStart(object sender, ProgressEventArgs ev) {
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    _chapterList.ScrollIntoView(ev.Item.LastDownloadedChapter);
                    _chapterList.SelectedItem = ev.Item.LastDownloadedChapter;

                    _progressBars[0].Maximum = ev.Total;
                    _progressBars[0].Value = ev.NumAtual;
                    _progressBars[1].Value = 0;

                    _downloadList.UpdateLayout();
                });
            });
        }

        private void OnDownloadChapterEnd(object sender, DownloadEventArgs ev) {

        }

        private void OnDownloadProgress(object sender, ProgressEventArgs ev) {
            Task.Run(() => {
                Dispatcher.Invoke(() => {

                    _progressBars[1].Maximum = ev.Total;
                    _progressBars[1].Value = ev.NumAtual;

                    _progress.Text = $"Baixando página {ev.NumAtual} de {ev.Total}...";

                    _downloadList.UpdateLayout();
                });
            });
        }

        private void OnDonwloadEnd(object sender, DownloadEventArgs ev) {
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    Shutdown();
                    _progress.Text = $"Download Concluido";
                });
            });
        }

        private void OnDownloadStart(object sender, DownloadEventArgs ev) {
            Task.Run(() => {
                Dispatcher.Invoke(() => {
                    _downloadListViewModel.ActualDownload = ev.Item;
                    _downloadList.ScrollIntoView(ev.Item);
                    var template = _downloadList.ItemContainerGenerator.ContainerFromItem(ev.Item);
                    _progressBars = template.Find<ProgressBar>();
                    _progress = template.Find<TextBlock>("Progress");
                });
            });
        }
    }
}
