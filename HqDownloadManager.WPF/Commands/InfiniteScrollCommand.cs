using HqDownloadManager.WPF.ViewModels;
using MangaScraping;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Commands {
    public class InfiniteScrollCommand : CommandBase<Dictionary<string, object>> {
        private SourceManager _sourceManager;

        public InfiniteScrollCommand(SourceManager sourceManager) {
            _sourceManager = sourceManager;
        }


        public override void Execute(Dictionary<string, object> dic) {
            var sourceLibraries = dic["LibraryViewModel"] as SourceLibraryViewModel;
            var source = _sourceManager.GetSources()[sourceLibraries.SelectedSource];
            if (dic.ContainsKey("VerticalOffset") && dic.ContainsKey("ScrollableHeight")) {
                var verticalOffset = Convert.ToInt32(dic["VerticalOffset"]);
                var scrollable = Convert.ToInt32(dic["ScrollableHeight"]);
                if (verticalOffset > 0 && (verticalOffset == scrollable)) {
                    var currentPage = ControlsHelper.GetCurrentPage();
                    Task.Run(() => {
                        var library = new List<Hq>();
                        source.NextLibraryPage(out library);
                        currentPage.Dispatcher.Invoke(() => {
                            foreach (var hq in library) {
                                sourceLibraries.HqLibrary.Add(hq);
                            }
                        });
                    });

                }
            }
        }
    }
}
