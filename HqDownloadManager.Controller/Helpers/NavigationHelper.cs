using HqDownloadManager.Controller.ViewModel.MainPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controller.Helpers {
    public class NavigationHelper {
        private readonly Frame _frame;
        private readonly ControlsHelper _controlsHelper;

        public NavigationHelper(Frame frame, ControlsHelper controlsHelper) {
            _frame = frame;
            _controlsHelper = controlsHelper;
        }

        public void Navigate<T>(string pageTitle) {
            _frame.Navigate(typeof(T));
            GettitleViewModel().Title = pageTitle;
        }

        public void Navigate<T>(string pageTitle, object extra) {
            _frame.Navigate(typeof(T), extra);
            GettitleViewModel().Title = pageTitle;
        }

        private NavigationViewModel GettitleViewModel() {
            return _controlsHelper.FindResource<NavigationViewModel>("TitleViewModel");
        }
    }
}
