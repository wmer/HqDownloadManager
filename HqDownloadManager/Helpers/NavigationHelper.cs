using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HqDownloadManager.ViewModels.MainPage;

namespace HqDownloadManager.Helpers {
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

        private PageTitleViewModel GettitleViewModel()
        {
            return _controlsHelper.FindResource<PageTitleViewModel>("TitleViewModel");
        }
    }
}
