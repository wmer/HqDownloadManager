using DependencyInjectionResolver;
using HqDownloadManager.Controller.CustomEvents;
using HqDownloadManager.Controller.Helpers;
using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        private static string driverPath;
        private NavigationHelper _navigationHelper;

        public App() {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            if (string.IsNullOrEmpty(driverPath)) {
                driverPath = this.GetAssetFolder().Result;
            }
            CoreConfiguration.BaseDirectory = ApplicationData.Current.LocalFolder.Path;
            CoreConfiguration.WebDriversLocation = driverPath;
            CoreConfiguration.DownloadLocation = $"{ CoreConfiguration.BaseDirectory}\\Downloads";
            CoreConfiguration.DatabaseLocation = $"{ CoreConfiguration.BaseDirectory}\\Databases";

            if (Window.Current.Content == null) {
                var dependencyInjection = new DependencyInjection();
                _rootFrame = new Frame();
                _rootFrame.NavigationFailed += OnNavigationFailed;
                NavigationEventHub.Navigated += OnNavigated;
                NavigationEventHub.Navigating += OnNavigating;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) { }
                _navigationHelper = dependencyInjection
                                    .DefineDependency<NavigationHelper>(0, _rootFrame)
                                    .Resolve<NavigationHelper>();

                var mainPage = dependencyInjection
                                    .DefineDependency<MainPage>(0, _rootFrame)
                                    .Resolve<MainPage>();

                Window.Current.Content = mainPage;
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    NavigationHelper.CanGoBack() ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            }
            if (_rootFrame.Content == null) {
                _navigationHelper.Navigate<UpdatesPage>("Atualizações");
            }

            Window.Current.Activate();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs) {
            if (!NavigationHelper.CanGoBack()) return;
            backRequestedEventArgs.Handled = true;
            NavigationHelper.GoBack();
        }


        private void OnNavigating(object sender, Controller.CustomEvents.NavigationEventArgs navigatingCancelEventArgs) {
        }

        private void OnNavigated(object sender, Controller.CustomEvents.NavigationEventArgs navigationEventArgs) {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                NavigationHelper.CanGoBack() ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Salvar o estado do aplicativo e parar qualquer atividade em segundo plano
            deferral.Complete();
        }

        private async Task<string> GetAssetFolder() {
            var driverPath = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets\WebDrivers");
            return driverPath.Path;
        }
    }
}
