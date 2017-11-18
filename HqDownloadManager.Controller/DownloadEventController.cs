using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download.CustomEventArgs;
using HqDownloadManager.FollowUpdate.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controller {
    public partial class DownloadController : ControllerBase {
        private void HandlerEvents() {
            DownloadManager.DownloadError += DownloadManagerOnDownloadError;
            DownloadManager.DownloadEnd += DownloadManagerOnDownloadEnd;
            DownloadManager.DownloadProgress += DownloadManagerOnDownloadProgress;
            DownloadManager.DownloadStart += DownloadManagerOnDownloadStart;
            DownloadManager.DownloadPause += DownloadManagerOnDownloadPause;
            DownloadManager.DownloadResume += DownloadManagerOnDownloadResume;
            UpdateManager.UpdateEnd += UpdateManagerOnUpdateEnd;
            UpdateManager.UpdateStart += UpdateManagerOnUpdateStart;
            FollowManager.FollowingHq += FollowManagerOnFollowingHq;
            ZipManager.CompressionError += ZipManager_CompressionError;
            ZipManager.CompressionEnd += ZipManager_CompressionEnd;
            ZipManager.CompressionStart += ZipManager_CompressionStart;
        }

        private void ShowLog(string msg) {
           Task.Run(()=> {
                Debug.WriteLine(msg);
            });
        }

        private async void DownloadManagerOnDownloadStart(object sender, DownloadEventArgs ev) {
            if (hqDownloading == null) return;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (ev.Item.GetType().IsAssignableFrom(typeof(Hq))) {
                    hqDownloading.TotalChapters =(ev.Item as Hq).Chapters.Count;
                }
                if (ev.Item.GetType().IsAssignableFrom(typeof(Chapter))) {
                    hqDownloading.DownloadChapter = ev.Item as Chapter;
                }

                var message = $"{ev.StartTime} - Iniciando download de {ev.Item.Title} em {ev.Path}";
                ShowLog(message);
                hqDownloading.Status = message;
                downloadListViewModel.Hqs[actualIndex] = hqDownloading;
            });
        }

        private async void DownloadManagerOnDownloadProgress(object sender, ProgressEventArgs ev) {
            if (hqDownloading == null) return;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                var mensagem = "";
                if (ev.Item.GetType().IsAssignableFrom(typeof(Hq))) {
                    hqDownloading.ActualChapter = ev.NumAtual;
                    mensagem = $"Capitulo {ev.NumAtual} de {ev.Total}";
                }
                if (ev.Item.GetType().IsAssignableFrom(typeof(Chapter))) {
                    hqDownloading.TotalPages = ev.Total;
                    hqDownloading.ActualPage = ev.NumAtual;
                    mensagem = $"Página {ev.NumAtual} de {ev.Total}";
                }

                mensagem = $"{ev.Time} - Baixando {ev.Item.Title}, {mensagem}";
                hqDownloading.Status = mensagem;
                downloadListViewModel.Hqs[actualIndex] = hqDownloading;
                ShowLog(mensagem);
            });

        }

        protected override void SourceManagerOnProcessingError(object sender, ProcessingErrorEventArgs ev) {
            base.SourceManagerOnProcessingError(sender, ev);
            if (hqDownloading == null) return;
            Task.Run(() => {
                ShowLog($"{ev.Time} - Houve uma falha no processamento do endereço {ev.Link} falhou. Causa: {ev.Exception}");
            });
        }

        private void DownloadManagerOnDownloadResume(object sender, ProgressEventArgs ev) {

        }

        private void DownloadManagerOnDownloadPause(object sender, ProgressEventArgs ev) {

        }

        private async void DownloadManagerOnDownloadEnd(object sender, DownloadEventArgs ev) {
            if (hqDownloading == null) return;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                ShowLog($"{ev.EndTime} - O doenload de {ev.Item.Title} terminou em {ev.TotalTime}");
                if (ev.Item.GetType().IsAssignableFrom(typeof(Hq))) {
                    hqDownloading.FinishedIn = DateTime.Now;
                }
            });

        }

        private void DownloadManagerOnDownloadError(object sender, DownloadErrorEventArgs ev) {
            if (hqDownloading == null) return;
            ShowLog($"{ev.Time} - O doenload de {ev.Item?.Title} falhou. Causa: {ev.Cause}");
        }

        private void FollowManagerOnFollowingHq(object sender, FollowEventArgs ev) {

        }

        private void UpdateManagerOnUpdateStart(object sender, UpdateEventArgs ev) {

        }

        private void UpdateManagerOnUpdateEnd(object sender, UpdateEventArgs ev) {

        }

        private void ZipManager_CompressionStart(object sender, Compression.CustomEventArgs.CompressionEventArgs ev) {

        }

        private void ZipManager_CompressionEnd(object sender, Compression.CustomEventArgs.CompressionEventArgs ev) {

        }

        private void ZipManager_CompressionError(object sender, Compression.CustomEventArgs.CompressionErrorEventArgs ev) {

        }
    }
}
