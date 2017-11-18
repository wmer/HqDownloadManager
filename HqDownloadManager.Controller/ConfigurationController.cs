using DependencyInjectionResolver;
using HqDownloadManager.Utils;
using HqDownloadManager.Controller.Models;
using HqDownloadManager.Controller.ViewModel.ConfigurationPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller {
    public class ConfigurationController : ControllerBase {
        public ConfigurationController(DependencyInjection dependencyInjection) : base(dependencyInjection) {
        }

        public override void Init(params object[] values) {
            base.Init();
            var userPreferences = GetUserPreferencesResource();
            userPreferences.Compress = UserPreferences.Compress;
            userPreferences.EraseFolder = UserPreferences.EraseFolder;
            userPreferences.DownloadPath = UserPreferences.DownloadPath;
            userPreferences.Shutdown = UserPreferences.Shutdown;
        }

        public async Task SetDefaultPathAsync() {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null) {
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                var folderPath = folder.Path;
                GetUserPreferencesResource().DownloadPath = folderPath;
                UserPreferences.DownloadPath = folderPath;
            }
        }

        public void SaveConfiguration() {
            UserPreferences = GetUserPreferencesResource();
            var userP = new UserPreferences { Id = 1, UserPreferencesViewModel = UserPreferences.ToBytes() };
            UserLibrary.UserPreferences.Update(userP);
            //notificationHelper.Notify("Configuração", "Configurações salvas com sucesso!");
        }

        private UserPreferencesViewModel GetUserPreferencesResource() {
            return ControlsHelper.FindResource<UserPreferencesViewModel>("Preferences"); ;
        }
    }
}
