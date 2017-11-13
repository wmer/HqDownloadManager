using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Download.CustomEventArgs {
    public class ProgressEventArgs : EventArgs {
        public DateTime Time { get; private set; }
        public ModelBase Item { get; private set; }
        public int NumAtual { get; private set; }
        public int Total { get; private set; }

        public ProgressEventArgs(DateTime time, ModelBase item, int numAtual, int total) {
            Time = time;
            Item = item;
            NumAtual = numAtual;
            Total = total;
        }
        public ProgressEventArgs(ModelBase item, int numAtual, int total) {
            Item = item;
            NumAtual = numAtual;
            Total = total;
        }
    }

    public delegate void ProgressEventHandler(object sender, ProgressEventArgs ev);
}
