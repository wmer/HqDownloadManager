using HqDownloadManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class ShortcutsReaderCommand : CommandBase<ReaderViewModel> {
        public override void Execute(ReaderViewModel parameter) {
            if (Keyboard.IsKeyDown(Key.C))
                parameter.DetailsVisibility = !parameter.DetailsVisibility;

            if (Keyboard.IsKeyDown(Key.B))
                parameter.ControlsVisibility = !parameter.ControlsVisibility;
        }
    }
}
