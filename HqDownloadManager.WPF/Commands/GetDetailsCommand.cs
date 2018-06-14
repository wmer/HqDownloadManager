using HqDownloadManager.WPF.UserControls;
using MangaScraping;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class GetDetailsCommand : CommandBase<Tuple<string, object, DetailsUserControl>> {
        private SourceManager _sourceManager;

        public GetDetailsCommand(SourceManager sourceManager) {
            _sourceManager = sourceManager;
        }

        public override void Execute(Tuple<string, object, DetailsUserControl> dic) {
            var hq = new Hq();
            var link = "";
            var detailsViewModel = dic.Item3;
            var source = _sourceManager.GetSources()[(dic.Item1)];
            if (dic.Item2 is Update update) {
                detailsViewModel.Hq = update.Hq;
                link = update.Hq.Link;
            } else if (dic.Item2 is Hq hqSelected) {
                detailsViewModel.Hq = hqSelected;
                link = hqSelected.Link;
            }
            if (!string.IsNullOrEmpty(link)) {
                Task.Run(() => {
                    source.GetInfo(link, out hq);
                    Application.Current.Dispatcher.Invoke(() => {
                        detailsViewModel.Hq = hq;
                    });
                });
            }
        }
    }
}
