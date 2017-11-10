using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Views;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controllers {
    public class HqDetailsController : Controller {
        public HqDetailsController(DependencyInjection dependencyInjection) : base(dependencyInjection) {
        }

        public void OpenReader(Hq model) {
            NavigationHelper.Navigate<HqReaderPage>("Leitor", model);
        }

        public async Task ReadNow(Hq hq) {
            var tempHq = hq;
           tempHq.Chapters = await GetSelectedChapters();
            OpenReader(tempHq);
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
