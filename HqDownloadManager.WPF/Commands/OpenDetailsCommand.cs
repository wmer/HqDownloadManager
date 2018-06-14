using HqDownloadManager.WPF.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class OpenDetailsCommand : CommandBase<DetailsUserControl> {

        public override bool CanExecute(DetailsUserControl detailsControl) {
            return detailsControl != null && detailsControl.Opened == false;
        }

        public override void Execute(DetailsUserControl detailsControl) => detailsControl.Opened = true;
    }
}
