using HqDownloadManager.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HqDownloadManager.Controllers {
    public abstract class Controller {
        protected ControlsHelper controlsHelper;
        protected NavigationHelper navigationHelper;
        protected ClickHelper clickHelper;

        public Controller(ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper) {
            this.controlsHelper = controlsHelper;
            this.navigationHelper = navigationHelper;
            this.clickHelper = clickHelper;
        }

        public void Click(object sender, MouseButtonEventArgs e, Action action) => clickHelper.Click(sender, e, action);
        public void SetTimesOfClick(object sender, MouseButtonEventArgs e) => clickHelper.MouseDown(sender, e);
    }
}
  