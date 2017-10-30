using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download.CustomEventArgs;

namespace HqDownloadManager.Controllers {
    public partial class DownloadController {

        private void HandlerEvents() {
            downloadManager.DownloadError += DownloadManagerOnDownloadError;
            downloadManager.DownloadEnd += DownloadManagerOnDownloadEnd;
            downloadManager.DownloadProgress += DownloadManagerOnDownloadProgress;
            downloadManager.DownloadStart += DownloadManagerOnDownloadStart;
            downloadManager.DownloadPause += DownloadManagerOnDownloadPause;
            downloadManager.DownloadResume += DownloadManagerOnDownloadResume;
            downloadManager.UpdateHq += DownloadManagerOnUpdateHq;
            downloadManager.UpdateEnd += DownloadManagerOnUpdateEnd;
            downloadManager.UpdateStart += DownloadManagerOnUpdateStart;
            downloadManager.FollowingHq += DownloadManagerOnFollowingHq;
            zipManager.CompressionError += ZipManager_CompressionError;
            zipManager.CompressionEnd += ZipManager_CompressionEnd;
            zipManager.CompressionStart += ZipManager_CompressionStart;
        }

        private void ShowLog(string msg) {
            if (logViewModel == null) return;
            dispatcher.Invoke(() => {
                var log = $"{logViewModel?.Log}\n{msg}";
                if (logViewModel != null) logViewModel.Log = log;
                var scrrol = controlsHelper.Find<ScrollViewer>("PortLog");
                scrrol?.ScrollToEnd();
            });
        }

        private void DownloadManagerOnDownloadStart(object sender, DownloadEventArgs ev) {
            if (hqDownloading == null) return;
            Task.Run(() => {
                if (ev.Item.GetType().IsAssignableFrom(typeof(Hq))) {
                    dispatcher.Invoke(() => {
                        hqDownloading.Hq = ev.Item as Hq;
                    });
                }
                if (ev.Item.GetType().IsAssignableFrom(typeof(Chapter))) {
                    dispatcher.Invoke(() => {
                        hqDownloading.Chapter = ev.Item as Chapter;
                    });
                }

                ShowLog($"{ev.StartTime} - Iniciando download de {ev.Item.Title} em {ev.Path}");
            });
        }

        private void DownloadManagerOnDownloadProgress(object sender, ProgressEventArgs ev) {
            if (hqDownloading == null) return;
            Task.Run(() => {
                dispatcher.Invoke(() => {
                    hqDownloading.Visibility = Visibility.Visible;
                });
                var mensagem = "";
                if (ev.Item.GetType().IsAssignableFrom(typeof(Hq))) {
                    dispatcher.Invoke(() => {
                        hqDownloading.ActualChapter = ev.NumAtual;
                    });
                    mensagem = $"Capitulo {ev.NumAtual} de {ev.Total}";
                }
                if (ev.Item.GetType().IsAssignableFrom(typeof(Chapter))) {
                    dispatcher.Invoke(() => {
                        hqDownloading.ActualPage = ev.NumAtual;
                    });
                    mensagem = $"Página {ev.NumAtual} de {ev.Total}";
                }

                ShowLog($"{ev.Time} - Baixando {ev.Item.Title}, {mensagem}");
            });

        }

        private void DownloadManagerOnDownloadResume(object sender, ProgressEventArgs ev) {

        }

        private void DownloadManagerOnDownloadPause(object sender, ProgressEventArgs ev) {

        }

        private void DownloadManagerOnDownloadEnd(object sender, DownloadEventArgs ev) {
            if (hqDownloading == null) return;
            Task.Run(() => {
                ShowLog($"{ev.EndTime} - O doenload de {ev.Item.Title} terminou em {ev.TotalTime}");
                if (ev.Item.GetType().IsAssignableFrom(typeof(Hq))) {
                    dispatcher.Invoke(() => {
                        hqDownloading.Hq = null;
                    });
                }
                if (ev.Item.GetType().IsAssignableFrom(typeof(Chapter))) {
                    dispatcher.Invoke(() => {
                        hqDownloading.Chapter = null;
                    });

                }
                dispatcher.Invoke(() => {
                    hqDownloading.Visibility = Visibility.Hidden;
                });
            });

        }

        private void DownloadManagerOnDownloadError(object sender, DownloadErrorEventArgs ev) {
            if (hqDownloading == null) return;
            Task.Run(() => {
                ShowLog($"{ev.Time} - O doenload de {ev.Item.Title} falhou. Causa: {ev.Cause}");
            });
        }

        private void DownloadManagerOnUpdateHq(object sender, UpdateEventArgs ev) {

        }

        private void DownloadManagerOnUpdateStart(object sender, UpdateEventArgs ev) {

        }

        private void DownloadManagerOnUpdateEnd(object sender, UpdateEventArgs ev) {

        }

        private void DownloadManagerOnFollowingHq(object sender, FollowEventArgs ev) {

        }

        private void ZipManager_CompressionStart(object sender, Compression.CustomEventArgs.CompressionEventArgs ev) {

        }

        private void ZipManager_CompressionEnd(object sender, Compression.CustomEventArgs.CompressionEventArgs ev) {

        }

        private void ZipManager_CompressionError(object sender, Compression.CustomEventArgs.CompressionErrorEventArgs ev) {

        }
    }
}
