using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.CustomEvents {
    public static class NavigationEventHub {
        public static event NavigationEventHandler Navigating;
        public static event NavigationEventHandler Navigated;

        public static void OnNavigated(object sender, NavigationEventArgs e) =>
                                                            Navigated.Invoke(sender, e);

        public static void OnNavigating(object sender, NavigationEventArgs e) =>
                                                            Navigating.Invoke(sender, e);
    }
}
