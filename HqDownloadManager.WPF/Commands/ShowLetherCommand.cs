using HqDownloadManager.WPF.ViewModels;
using MangaScraping;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class ShowLetherCommand : CommandBase<SourceLibraryViewModel> {
        private SourceManager _sourceManager;

        public ShowLetherCommand(SourceManager sourceManager) {
            _sourceManager = sourceManager;
        }

        public override bool CanExecute(SourceLibraryViewModel parameter) =>
                                    parameter != null && parameter.SelectedLether != null;

        public override void Execute(SourceLibraryViewModel sourceLibraries) {
            var source = _sourceManager.GetSources()[sourceLibraries.SelectedSource];
            Task.Run(() => {
                source.GetLetherPage(sourceLibraries.SelectedLether, out List <Hq> library);
                sourceLibraries.HqLibrary = new ObservableCollection<Hq>(library);
            });
        }
    }
}
