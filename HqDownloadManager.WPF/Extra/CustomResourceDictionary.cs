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
            Register<MainWindowViewModel>("MainWindow");
            Register<NavigationViewModel>("NavigationView");
            Register<SourceUpdateViewModel>("Updates");
            Register<SourceLibraryViewModel>("Libraries");
            Register<MyLibraryViewModel>("MyLibrary");
            Register<DownloadDetailsViewModel>("DownloadDetails");
        }
    }
}
