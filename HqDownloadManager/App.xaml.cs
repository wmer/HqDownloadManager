using DependencyInjectionResolver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HqDownloadManager {
    /// <summary>
    ///Fornece o comportamento específico do aplicativo para complementar a classe Application padrão.
    /// </summary>
    sealed partial class App : Application {
        private Frame _rootFrame;

        public App() {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            if (Window.Current.Content == null) {
                var dependencyInjection = new DependencyInjection();
                _rootFrame = new Frame();
                _rootFrame.NavigationFailed += OnNavigationFailed;
                _rootFrame.Navigated += OnNavigated;
                _rootFrame.Navigating += OnNavigating;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) { }

                var mainPage = dependencyInjection
                                    .DefineDependency<MainPage>(0, _rootFrame)
                                    //.DefineDependency<NavigationHelper>(0, _rootFrame)
                                    .Resolve<MainPage>();
                Window.Current.Content = mainPage;
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    _rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            }
            if (_rootFrame.Content == null) {
               // _rootFrame.Navigate(typeof(UpdatesPage), e.Arguments);
               // ((Window.Current.Content as MainPage)?.Resources["TitleViewModel"] as PageTitleViewModel).Title = "Atualizações";
            }

            Window.Current.Activate();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs) {
            if (_rootFrame == null || !_rootFrame.CanGoBack) return;
            backRequestedEventArgs.Handled = true;
            _rootFrame.GoBack();
        }


        private void OnNavigating(object sender, NavigatingCancelEventArgs navigatingCancelEventArgs) {
        }

        private void OnNavigated(object sender, NavigationEventArgs navigationEventArgs) {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        
        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Salvar o estado do aplicativo e parar qualquer atividade em segundo plano
            deferral.Complete();
        }
    }
}
