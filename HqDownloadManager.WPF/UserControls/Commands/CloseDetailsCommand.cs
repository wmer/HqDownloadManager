using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.UserControls.Commands {
    public class CloseDetailsCommand : CommandBase<DetailsUserControl> {
        public override void Execute(DetailsUserControl parameter) => parameter.Opened = false;
    }
}
