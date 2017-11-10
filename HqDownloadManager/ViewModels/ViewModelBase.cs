using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace HqDownloadManager.ViewModels {
    public class ViewModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public async void OnPropertyChanged(string property) {
            if (Window.Current is Window window) {
                await window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
                });
            }
        }
    }
}
