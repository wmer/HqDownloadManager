using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HqDownloadManager.Helpers {
    public class ClickHelper {
        private DateTime downTime;
        private object downSender;

        public void Click(object sender, MouseButtonEventArgs e, Action action = null) {
            if (e.LeftButton != MouseButtonState.Released || sender != this.downSender) return;
            var timeSinceDown = DateTime.Now - this.downTime;
            if (timeSinceDown.TotalMilliseconds < 500) {
                action?.Invoke();
            }
        }

        public void MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            this.downSender = sender;
            this.downTime = DateTime.Now;
        }
    }
}
