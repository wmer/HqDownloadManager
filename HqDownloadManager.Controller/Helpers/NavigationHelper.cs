using DependencyInjectionResolver;
using HqDownloadManager.Controller.CustomEvents;
using HqDownloadManager.Controller.ViewModel.MainPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controller.Helpers {
    public class NavigationHelper {
        private readonly Frame _frame;
        private readonly ControlsHelper _controlsHelper;
        private readonly DependencyInjection _dependencyInjection;

        private static Page _previousPage;
        private static Page _actualPage;
        private static Page _nextPage;

        private object _extraContent;
        private string _pageTitle;

        public NavigationHelper(Frame frame, ControlsHelper controlsHelper, DependencyInjection dependencyInjection) {
            _frame = frame;
            _controlsHelper = controlsHelper;
            _dependencyInjection = dependencyInjection;
        }


        public void Navigate<T>(string pageTitle) where T : Page => Navigate<T>(pageTitle, null);

        public void Navigate<T>(string pageTitle, object extra) where T : Page {
            if (_frame.Content != null && _frame.Content is Page pg) {
                _previousPage = pg;
            }
            var page = _dependencyInjection.Resolve<T>();
            NavigationEventHub.OnNavigating(_previousPage, new NavigationEventArgs(pageTitle, page, extra));
            _actualPage = page;
            _frame.Content = page;
            _extraContent = extra;
            GettitleViewModel().Title = pageTitle;
            _pageTitle = pageTitle;
            page.Loaded += Page_Loaded;
        }

        public static bool CanGoBack() => _previousPage != null;

        public static bool GoBack() {
            var navigated = false;
            if (CanGoBack()) {
                if (Window.Current.Content is Page mainPage && mainPage.FindName("Navigation") is NavigationView navigationView) {
                    if (navigationView.Content is Frame mainFrame) {
                        _nextPage = _actualPage;
                        _actualPage = _previousPage;
                        _previousPage = null;
                        mainFrame.Content = _actualPage;
                        navigated = true;
                    }
                }
            }
            return navigated;
        }

        private NavigationViewModel GettitleViewModel() {
            return _controlsHelper.FindResource<NavigationViewModel>("TitleViewModel");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            NavigationEventHub.OnNavigated(_previousPage, new NavigationEventArgs(_pageTitle, _previousPage, _actualPage, _extraContent));
        }
    }
}
