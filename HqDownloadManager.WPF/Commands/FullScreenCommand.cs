using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class FullScreenCommand : CommandBase<KeyEventArgs> {
        public override bool CanExecute(KeyEventArgs parameter) =>
                                    parameter.OriginalSource is Window window &&
                                    window.ResizeMode == ResizeMode.CanResize &&
                                    Keyboard.IsKeyDown(Key.LeftAlt) && 
                                    Keyboard.IsKeyDown(Key.Enter);

        public override void Execute(KeyEventArgs parameter) {
            var window = parameter.OriginalSource as Window;
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
            window.WindowState = WindowState.Maximized;
            window.Left = 0;
            window.Top = 0;
            window.Width = SystemParameters.FullPrimaryScreenWidth;
            window.Height = SystemParameters.FullPrimaryScreenHeight;
            window.Topmost = true;
        }
    }

    public class ExitFullScreenCommand : CommandBase<KeyEventArgs> {
        public override bool CanExecute(KeyEventArgs parameter) =>
                                    parameter.OriginalSource is Window window &&
                                    window.ResizeMode == ResizeMode.NoResize &&
                                    Keyboard.IsKeyDown(Key.Escape);

        public override void Execute(KeyEventArgs parameter) {
            var window = parameter.OriginalSource as Window;
            window.WindowStyle = WindowStyle.ThreeDBorderWindow;
            window.ResizeMode = ResizeMode.CanResize;
            window.WindowState = WindowState.Normal;
            window.Left = 10;
            window.Top = 10;
            window.Width = 900;
            window.Height = 600;
            window.Topmost = false;
        }
    }
}
