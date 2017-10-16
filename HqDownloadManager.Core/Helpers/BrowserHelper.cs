using SHDocVw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Helpers {
    internal class BrowserHelper {
        private Object lockThis = new Object();

        public InternetExplorer GetInternetExplorer(string url, bool visible = false, bool fullScreen = false) {
            lock (lockThis) {
                var IE = new InternetExplorer() {
                    Visible = visible
                };
                IE.Navigate(url);
                IE.FullScreen = fullScreen;
                while (IE.Busy) ;
                while (IE.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) ;
                return IE;
            }
        }
    }
}
