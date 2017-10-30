using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Core;
using HqDownloadManager.Database;
using HqDownloadManager.Download;
using HqDownloadManager.Helpers;
using System.Windows.Forms;
using HqDownloadManager.Compression;
using HqDownloadManager.Models;

namespace HqDownloadManager.Controllers {
    public class ConfigurationController : Controller {

        public ConfigurationController(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper, ZipManager zipManager) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper, zipManager) {
        }

        public override void Init(params object[] values) {
            userPreferences = GetUserPreferencesResource();
            if (userLibrary.UserPreferences.FindOne(1) is UserPreferences userP) {
                userPreferences.Compress = userP.Compress;
                userPreferences.EraseFolder = userP.EraseFolder;
                userPreferences.DownloadPath = userP.DownloadPath;
                userPreferences.Shutdown = userP.Shutdown;
                userPreferences.Id = userP.Id;
            }
        }

        [STAThread]
        public void SetDefaultPath() {
            var defaultPath = userPreferences.DownloadPath;
            Task<string>.Factory.StartNew(() => {
                var selectedPath = "";
                var selectFolderDialog = new FolderBrowserDialog();
                selectFolderDialog.SelectedPath = userPreferences.DownloadPath;
                dispatcher.Invoke(() => {
                    if (selectFolderDialog.ShowDialog() == DialogResult.OK) {
                        selectedPath = selectFolderDialog.SelectedPath;
                    }
                });
              
                return selectedPath;
            }).ContinueWith((path)=> {
                if (!string.IsNullOrEmpty(path.Result)) {
                    dispatcher.Invoke(() => {
                        userPreferences.DownloadPath = path.Result;
                    });
                }
            });
        }

        public void SaveConfiguration() {
            userPreferences = GetUserPreferencesResource();
            userLibrary.UserPreferences.Update(userPreferences);
            notificationHelper.Notify("Configuração", "Configurações salvas com sucesso!");
        }

        private UserPreferences GetUserPreferencesResource() {
            return controlsHelper.FindResource<UserPreferences>("Preferences"); ;
        }
    }
}
