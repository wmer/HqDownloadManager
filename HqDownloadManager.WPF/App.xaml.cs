using DependencyInjectionResolver;
using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Download;
using HqDownloadManager.Download.Configuration;
using HqDownloadManager.Shared.Database;
using HqDownloadManager.Shared.Models;
using HqDownloadManager.WPF.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.UI.Core;
using WPF.Tools.Navigation;
using WPF.Tools.Navigation.Events;

namespace HqDownloadManager.WPF {
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application {
        private Frame _rootFrame;
        private NavigationManager _navigationManager;
        private DependencyInjection _dependemcyInjection;

        protected override void OnStartup(StartupEventArgs e) {
            _rootFrame = new Frame();
            _dependemcyInjection = new DependencyInjection();
            DefineConfigurations();
            _navigationManager = new NavigationManager(_dependemcyInjection);
            var window = _dependemcyInjection
                            .DefineDependency<MainWindow>(0, _rootFrame).Resolve<MainWindow>();
            window.Top = 50;
            window.Left = 50;
            Current.MainWindow = window;

            NavigationEventHub.Navigated += OnNavigated;
            NavigationEventHub.Navigating += OnNavigating;
            NavigationEventHub.NavigationFailed += OnNavigationFailed; ;
            
            if (_rootFrame.Content == null) {
                _rootFrame.Navigate(_dependemcyInjection.Resolve<SourceUpdatesPage>());
                NavigationEventHub.OnNavigated(this, new global::WPF.Tools.Navigation.Events.NavigationEventArgs("Updates", null));
            }
            window.Show();
        }

        private void DefineConfigurations() {
            var context = _dependemcyInjection.Resolve<ConfigurationContext>();
            if (context.Configuration.FindOne("1") is Configuration config) {
                config.DownloadLocations = context.DownloadLocation.FindAll();
                CoreConfiguration.CacheLocation = config.CacheLocation;
                CoreConfiguration.DatabaseLocation = config.DatabaseLocation;
                CoreConfiguration.WebDriversLocation = config.WebdriverLocation;
                var list = new List<string>();
                foreach (var loc in config.DownloadLocations) { 
                    list.Add(loc.Location);
                }
                DownloadConfiguration.DownloadLocations = list;
            } else {
                var list = new List<DownloadLocation>();
                var configuration = new Configuration {
                    CacheLocation = CoreConfiguration.CacheLocation,
                    DatabaseLocation = CoreConfiguration.DatabaseLocation,
                    WebdriverLocation = CoreConfiguration.WebDriversLocation
                };
                foreach (var loc in  DownloadConfiguration.DownloadLocations) {
                    list.Add(new DownloadLocation { Location = loc });
                }
                context.DownloadLocation.Save(list);
                configuration.DownloadLocations = list;
                context.Configuration.Save(configuration);
            }
        }

        private void OnNavigating(object sender, global::WPF.Tools.Navigation.Events.NavigationEventArgs e) {

        }

        private void OnNavigated(object sender, global::WPF.Tools.Navigation.Events.NavigationEventArgs e) {

        }

        private void OnNavigationFailed(object sender, global::WPF.Tools.Navigation.Events.NavigationFailedEventArgs e) {
            throw new Exception($"Failed to load Page {e.DestinationPageType.FullName}, causa: {e.Error.Message}");
        }
    }
}
