using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Commands {
    public class ScrollHorizontallyCommand : CommandBase<MouseWheelEventArgs> {
        const int WM_MOUSEHWHEEL = 0x020E;
        private ListView _listView;
        private int _tilt;

        public override void Execute(MouseWheelEventArgs parameter) {
            _listView = ControlsHelper.Find<ListView>("FlipViewReader");

            var source = PresentationSource.FromVisual(_listView);
            ((HwndSource)source)?.AddHook(Hook);
        }

        private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            switch (msg) {
                case WM_MOUSEHWHEEL:
                    int tilt = (short)HIWORD(wParam);
                    OnMouseTilt(tilt);
                    return (IntPtr)1;
            }
            return IntPtr.Zero;
        }

        private static int HIWORD(IntPtr ptr) {
            var val32 = ptr.ToInt32();
            return ((val32 >> 16) & 0xFFFF);
        }

        /// <summary>
        /// Gets low bits values of the pointer.
        /// </summary>
        private static int LOWORD(IntPtr ptr) {
            var val32 = ptr.ToInt32();
            return (val32 & 0xFFFF);
        }

        private void OnMouseTilt(int tilt) {
            var scrollviewer = _listView.Find<ScrollViewer>().FirstOrDefault();
            if (tilt < 0 && tilt != _tilt) {
                scrollviewer.LineLeft();
            }
            else if (tilt > 0 && tilt != _tilt) {
                scrollviewer.LineRight();
                if (_listView.SelectedIndex > 0) {
                }
            }

            _tilt = tilt;
        }
    }
}
