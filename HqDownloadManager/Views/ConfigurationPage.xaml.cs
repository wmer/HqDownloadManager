using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DependencyInjectionResolver;
using HqDownloadManager.Controllers;

namespace HqDownloadManager.Views {
    /// <summary>
    /// Interação lógica para ConfigurationPage.xam
    /// </summary>
    public partial class ConfigurationPage : PageControllerBase<ConfigurationController> { 

        public ConfigurationPage(DependencyInjection dependencyInjection) : base(dependencyInjection) {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
            base.OnLoaded(sender, routedEventArgs);
        }

        [STAThread]
        private void btnAlterLocDownload_Click(object sender, RoutedEventArgs e) => Controller.SetDefaultPath();

        private void saveConfigs_Click(object sender, RoutedEventArgs e) => Controller.SaveConfiguration();
    }
}
