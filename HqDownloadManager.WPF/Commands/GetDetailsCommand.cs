using DependencyInjectionResolver;
using HqDownloadManager.WPF.ViewModels;
using MangaScraping;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class GetDetailsCommand : CommandBase<Dictionary<string, object>> {
        private SourceManager _sourceManager;

        public GetDetailsCommand() {
            var injectionResolver = new DependencyInjection();
            _sourceManager = injectionResolver.Resolve<SourceManager>();
        }

        public override void Execute(object parameter) {
            if (parameter is Dictionary<string, object> dic) {
                var hq = new Hq();
                var update = dic["SelectedUpdate"] as Update;
                var source = _sourceManager.GetSources()[((string)dic["SelectedSource"])];
                Task.Run(()=> {
                    source.GetInfo<Hq>(update.Hq.Link, out hq);
                    if (dic["DetailsViewModel"] is DetailsViewModel detailsViewModel) {
                        detailsViewModel.Hq = hq;
                    }
                });
            }
        }
    }
}
