using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Download;
using HqDownloadManager.Shared.Database;
using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.COnfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Controller {
    public class ConfigurationController : System.Windows.Controls.Page {
        private ConfigurationContext _configurationContext;
        private ConfigurationViewModel _configurationViewModel;

        public ConfigurationController(ConfigurationContext configurationContext) {
            _configurationContext = configurationContext;
        }

        public void OnLoaded(object sender, RoutedEventArgs e) {
            _configurationViewModel = ControlsHelper.FindResource<ConfigurationViewModel>("Configuration");
            if (_configurationContext.Configuration.FindOne("1") is Configuration config) {
                config.DownloadLocations = _configurationContext.DownloadLocation.FindAll();
                _configurationViewModel.Configuartion = config;
            }
        }

        public void ChangeDbLocation() {
            var newLoc = SetDefaultPath();
            _configurationViewModel.Configuartion.DatabaseLocation = newLoc;
            _configurationContext.Configuration.Update(_configurationViewModel.Configuartion);
        }

        public void ChangeCacheLocation() {
            var newLoc = SetDefaultPath();
            _configurationViewModel.Configuartion.CacheLocation = newLoc;
            _configurationContext.Configuration.Update(_configurationViewModel.Configuartion);
        }

        public void ChangeWebDriverLocation() {
            var newLoc = SetDefaultPath();
            _configurationViewModel.Configuartion.WebdriverLocation = newLoc;
            _configurationContext.Configuration.Update(_configurationViewModel.Configuartion);
        }

        public void AddDownloadLocation() {
            var newLoc = SetDefaultPath();
            var dwLoc = new DownloadLocation { Location = newLoc };
            if (!(_configurationContext.DownloadLocation.Find().Where(x => x.Location == newLoc).Execute().FirstOrDefault() is DownloadLocation)) {
                _configurationViewModel.Configuartion.DownloadLocations.Add(dwLoc);
                _configurationContext.DownloadLocation.Save(dwLoc);
            }
        }

        public void DeleteDownloadLocation() {
            if (_configurationViewModel.SelectedPath is DownloadLocation dwLoc) {
                _configurationViewModel.Configuartion.DownloadLocations.Remove(dwLoc);
                _configurationContext.DownloadLocation.Delete(dwLoc);
            }
        }

        [STAThread]
        private string SetDefaultPath() {
            var selectedPath = "";
            var selectFolderDialog = new FolderBrowserDialog();
            if (selectFolderDialog.ShowDialog() == DialogResult.OK) {
                selectedPath = selectFolderDialog.SelectedPath;
            }
            return selectedPath;
        }
    }
}
