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
using HqDownloadManager.Models;

namespace HqDownloadManager.Controllers {
    public class ConfigurationController : Controller {
        UserPreferences userPreferences;

        public ConfigurationController(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper) : base(dependencyInjection, controlsHelper, navigationHelper, clickHelper, sourceManager, userLibrary, downloadManager, notificationHelper) {
        }

        public override void Init(params object[] values) {
            userPreferences = GetUserPreferencesResource();
            if (userLibrary.UserPreferences.FindOne(1) is UserPreferences userP) {
                userPreferences.Compress = userP.Compress;
                userPreferences.EraseFolder = userP.EraseFolder;
                userPreferences.DownloadPath = userP.DownloadPath;
                userPreferences.Id = userP.Id;
            }else {
                userPreferences.Compress = false;
                userPreferences.EraseFolder = false;
                userPreferences.DownloadPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Downloads\\Mangas";
                userLibrary.UserPreferences.Save(userPreferences);
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
