using DependencyInjectionResolver;
using HqDownloadManager.WPF.ViewModels;
using MangaScraping;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class GetDetailsCommand : CommandBase<Dictionary<string, object>> {
        private SourceManager _sourceManager;

        public GetDetailsCommand(SourceManager sourceManager) {
            _sourceManager = sourceManager;
        }

        public override bool CanExecute(Dictionary<string, object> dic) => dic != null && dic.Count() > 0;

        public override void Execute(Dictionary<string, object> dic) {
            var hq = new Hq();
            var link = "";
            var detailsViewModel = dic["DetailsViewModel"] as DetailsViewModel;
            var source = _sourceManager.GetSources()[((string)dic["SelectedSource"])];
            if (dic["SelectedItem"] is Update update) {
                detailsViewModel.Hq = update.Hq;
                link = update.Hq.Link;
            } else if (dic["SelectedItem"] is Hq hqSelected) {
                detailsViewModel.Hq = hqSelected;
                link = hqSelected.Link;
            }
            if (!string.IsNullOrEmpty(link)) {
                Task.Run(() => {
                    source.GetInfo<Hq>(link, out hq);
                    detailsViewModel.Hq = hq;
                });
            }
        }
    }
}
