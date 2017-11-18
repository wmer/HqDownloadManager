using DependencyInjectionResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Controller {
    public abstract class PageControllerBase<T> : Page where T : ControllerBase {
        protected T Controller;

        public PageControllerBase() {
            Loaded += OnLoaded; ;
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e) {
            Controller = new DependencyInjection().Resolve<T>();
            Controller.Init();
        }

    }
}
