using DependencyInjectionResolver;
using HqDownloadManager.WPF.Models;
using HqDownloadManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Extra;

namespace HqDownloadManager.WPF.Extra {
    public class CustomResourceDictionary : ResourceDictionary {        
        public override void RegisterResources() {
            Register<SourceUpdateViewModel>("Updates");
            Register<SourceLibraryViewModel>("Libraries");
            Register<DetailsViewModel>("DetailsView");
            Register<DownloadDetailsViewModel>("DownloadDetails");
            Register<MyLibraryViewModel>("MyLibrary");
            Register<DownloadListViewModel>("DownloadListView");
            Register<ChapterReadingProgress>("ReadingProgress");
            Register<ReaderViewModel>("ReaderViewModel");
        }
    }
}
 