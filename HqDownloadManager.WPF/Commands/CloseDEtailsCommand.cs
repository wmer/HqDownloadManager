using HqDownloadManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class CloseDetailsCommand : CommandBase {
        public override bool CanExecute(object parameter) {
            return parameter is DetailsViewModel viewModel && viewModel.Opened == true;
        }

        public override void Execute(object parameter) =>
                            (parameter as DetailsViewModel).Opened = false;
    }
}
