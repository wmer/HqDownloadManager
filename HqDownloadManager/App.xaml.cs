using DependencyInjectionResolver;
using HqDownloadManager.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace HqDownloadManager {
    public partial class App : Application {
        private Frame _rootFrame;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            _rootFrame = new Frame();
            _rootFrame.Navigating += OnNavigating;
            _rootFrame.Navigated += OnNavigated;
            _rootFrame.NavigationFailed += OnNavigationFailed;
            var dependencyInjection = new DependencyInjection();
            var window = dependencyInjection
                .DefineDependency<MainWindow>(0, _rootFrame).Resolve<MainWindow>();
            window.Top = 5;
            window.Left = 5;
            Current.MainWindow = window;
            if (_rootFrame.Content == null) {
                _rootFrame.Navigate(dependencyInjection.Resolve<HqUpdatesPage>());
                window.hqUpdates.IsChecked = true;
            }
            window.Show();
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs navigatingCancelEventArgs) {

        }

        private void OnNavigated(object sender, NavigationEventArgs navigationEventArgs) {

        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.Uri);
        }

    }
}
