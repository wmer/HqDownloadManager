using HqDownloadManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Events {
    public class ProgressEventArgs : EventArgs {
        public DateTime Time { get; private set; }
        public DownloadItem Item { get; private set; }
        public int NumAtual { get; private set; }
        public int Total { get; private set; }

        public ProgressEventArgs(DateTime time, DownloadItem item, int numAtual, int total) {
            Time = time;
            Item = item;
            NumAtual = numAtual;
            Total = total;
        }
        public ProgressEventArgs(DownloadItem item, int numAtual, int total) {
            Item = item;
            NumAtual = numAtual;
            Total = total;
        }
        public ProgressEventArgs(DateTime time, int numAtual, int total) {
            Time = time;
            NumAtual = numAtual;
            Total = total;
        }
        public ProgressEventArgs(int numAtual, int total) {
            NumAtual = numAtual;
            Total = total;
        }

    }

    public delegate void ProgressEventHandler(object sender, ProgressEventArgs ev);
}
