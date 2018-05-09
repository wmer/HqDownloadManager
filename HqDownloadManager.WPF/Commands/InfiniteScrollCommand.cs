using DependencyInjectionResolver;
using MangaScraping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class InfiniteScrollCommand : CommandBase<Dictionary<string, object>> {
        private SourceManager _sourceManager;

        public InfiniteScrollCommand() {
            var injectionResolver = new DependencyInjection();
            _sourceManager = injectionResolver.Resolve<SourceManager>();
        }


        public override void Execute(object parameter) {
            var a = "";
        }
    }
}
