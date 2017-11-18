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

        public UpdatesController(DependencyInjection dependencyInjection) : base(dependencyInjection) {
        }


        public override void Init(params object[] values) {
            base.Init();
            _hqList = ControlsHelper.FindResource<HqListViewModel>("HqList")?.Hqs;
        }

        public async Task ShowHqUpdates() {
            await Task.Run(async ()=> {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _hqList.Clear();
                    Notification.Visibility = Visibility.Visible;

                    var link = GetLinkForUpdates();
                    var updates = new List<Hq>();
                    if (!string.IsNullOrEmpty(link)) {
                        updates = SourceManager.GetUpdates(link);
                    }

                    foreach (var hq in updates) {
                        if (!String.IsNullOrEmpty(hq.Link)) {
                            _hqList.Add(hq);
                        }
                    }


                    Notification.Visibility = Visibility.Collapsed;
                });
            });         
        }

        public void ClearList() => _hqList.Clear();
    }
}
 