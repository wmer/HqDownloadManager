using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Controller.ViewModel.ComboBox;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Core.Models;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace HqDownloadManager.Controller {
    public class UpdatesController : ListHqControllerBase {
        private ObservableCollection<Hq> _hqList;

        public UpdatesController() : base() {
        }

        public override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            _hqList = ControlsHelper.FindResource<HqListViewModel>("HqList")?.Hqs;
        }

        public async Task ShowHqUpdates() {
            if (Notification == null) return;
            await Task.Run(async ()=> {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _hqList?.Clear();
                    Notification.Visibility = Visibility.Visible;

                    var link = GetLinkForUpdates();
                    var updates = new List<Update>();
                    if (!string.IsNullOrEmpty(link)) {
                        updates = SourceManager.GetUpdates(link);
                    }

                    foreach (var update in updates) {
                        if (!String.IsNullOrEmpty(update.Hq.Link)) {
                            _hqList.Add(update.Hq);
                        }
                    }


                    Notification.Visibility = Visibility.Collapsed;
                });
            });         
        }

        public void ClearList() => _hqList.Clear();
    }
}
 