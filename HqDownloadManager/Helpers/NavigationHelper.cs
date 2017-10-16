using DependencyInjectionResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HqDownloadManager.Helpers {
    public class NavigationHelper {
        private readonly Frame _mainFrame;

        public NavigationHelper(ControlsHelper controlsHelper) {
            var dockPanel = controlsHelper.Find<DockPanel>("Content");
            _mainFrame = dockPanel.Children[0] as Frame;
        }

        public DependencyInjection Configuration() {
            return new DependencyInjection();
        }

        public void Navigate<T>(DependencyInjection configuration = null) where T : Page {
            _mainFrame.Navigate(CreatePage<T>(configuration));
        }

        public void Navigate<T>(object data) where T : Page {

            _mainFrame.Navigate(CreatePage<T>(
                Configuration().DefineDependency<T>(0, data)));
        }

        private Page CreatePage<T>(DependencyInjection configuration = null) where T : Page {
            if (configuration == null) {
                configuration = Configuration();
            }
            return configuration.Resolve<T>();
        }
    }
}
