using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Core.Models;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controller {
    public class HqDetailsController : ControllerBase {
        public HqDetailsController(DependencyInjection dependencyInjection) : base(dependencyInjection) {
        }

        public void OpenReader<T>(Hq model) {
            NavigationHelper.Navigate<T>("Leitor", model);
        }

        public async Task ReadNow<T>(Hq hq) {
            var tempHq = hq;
            tempHq.Chapters = await GetSelectedChapters();
            OpenReader<T>(tempHq);
        }

        public async Task AddSelected(Hq hq) {
            var tempHq = hq;
            tempHq.Chapters = await GetSelectedChapters();
            await AddToDownloadList(tempHq);
        }

        private async Task<List<Chapter>> GetSelectedChapters() {
            var listChapters = new List<Chapter>();
            ListView list = null;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                list = ControlsHelper.Find<ListView>("HqChapters");
                var seleteds = list.SelectedItems;
                foreach (var item in seleteds) {
                    listChapters.Add(item as Chapter);
                }
            });
            return listChapters;
        }
    }
}
