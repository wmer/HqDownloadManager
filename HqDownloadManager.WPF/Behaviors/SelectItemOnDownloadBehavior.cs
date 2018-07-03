using HqDownloadManager.Events;
using HqDownloadManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace HqDownloadManager.WPF.Behaviors {
    public class SelectItemOnDownloadBehavior : Behavior<ListView> {
        protected override void OnAttached() {
            base.OnAttached();
            DownloadEventHub.DownloadStart += OnDownloadStart;
            DownloadEventHub.DownloadProgress += OnDownloadProgress;
        }

        private void OnDownloadProgress(object sender, ProgressEventArgs ev) => SelectItem(ev.Item);

        private void OnDownloadStart(object sender, DownloadEventArgs ev) => SelectItem(ev.Item);

        private void SelectItem(DownloadItem item) {
            Application.Current.Dispatcher.Invoke(() => {
                this.AssociatedObject.ScrollIntoView(item);
                this.AssociatedObject.SelectedItem = item;
            });
        }
    }
}
