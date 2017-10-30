using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DependencyInjectionResolver;
using HqDownloadManager.Controllers;

namespace HqDownloadManager.Views {
    public abstract class PageControllerBase<T> : Page where T : Controller {
        protected readonly DependencyInjection Dependency;
        protected T Controller;

        public PageControllerBase() {

        }

        public PageControllerBase(DependencyInjection dependency) {
            Dependency = dependency;
            Loaded += OnLoaded;
        }

        protected virtual void OnLoaded(object sender, System.Windows.RoutedEventArgs e) {
            Controller = Dependency.Resolve<T>();
            Controller.Init();
        }
    }
}
