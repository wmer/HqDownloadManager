using HqDownloadManager.WPF.UserControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Extra;

namespace HqDownloadManager.WPF.UserControls.Extra {
    public class UserControlsResourceDictionary : ResourceDictionary {
        public override void RegisterResources() {
            Register<DetailsViewModel>("DetailsView");
        }
    }
}
