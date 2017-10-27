using HqDownloadManager.Core;
using HqDownloadManager.Helpers;
using HqDownloadManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using DependencyInjectionResolver;
using HqDownloadManager.Database;
using HqDownloadManager.Download;

namespace HqDownloadManager.Controllers {
    public abstract class Controller {
        protected ControlsHelper controlsHelper;
        protected NavigationHelper navigationHelper;
        protected ClickHelper clickHelper;
        protected SourceManager sourceManager;
        protected DependencyInjection dependencyInjection;
        protected UserLibraryContext userLibrary;
        protected DownloadManager downloadManager;
        protected NotificationHelper notificationHelper;

        protected readonly Dispatcher dispatcher;
        protected NotificationViewModel notification;

        protected Controller(DependencyInjection dependencyInjection, ControlsHelper controlsHelper, NavigationHelper navigationHelper, ClickHelper clickHelper, SourceManager sourceManager, UserLibraryContext userLibrary, DownloadManager downloadManager, NotificationHelper notificationHelper) {
            this.dependencyInjection = dependencyInjection;
            this.controlsHelper = controlsHelper;
            this.navigationHelper = navigationHelper;
            this.clickHelper = clickHelper;
            this.userLibrary = userLibrary;
            this.downloadManager = downloadManager;
            this.notificationHelper = notificationHelper;
            this.sourceManager = sourceManager;
            this.sourceManager.ProcessingProgress += SourceManager_ProcessingProgress;

            dispatcher = Application.Current.MainWindow?.Dispatcher;
            notification = controlsHelper.FindResource<NotificationViewModel>("Notification");
        }

        public virtual void Init(params object[] values) {

        }

        protected string GetLinkForUpdates() {
            var sourceSelected = GetSourceSelectedFromComboBox();
            var link = "";
            switch (sourceSelected) {
                case "MangaHost":
                    link = "https://mangashost.com/";
                    break;
                case "YesMangas":
                    link = "https://ymangas.com/";
                    break;
                case "UnionMangas":
                    link = "http://unionmangas.net";
                    break;
                case "MangasProject":
                    link = "https://mangas.zlx.com.br";
                    break;
            }
            return link;
        }

        protected string GetSourceSelectedFromComboBox() {
            ComboBox _souceSelector = null;
            dispatcher.Invoke(() => {
                _souceSelector = controlsHelper.Find<ComboBox>("SourceHq");
            });

            ComboBoxItem itemSelected = null;
            String itemSelectedontent = null;
            dispatcher.Invoke(() => {
                itemSelected = _souceSelector.SelectedItem as ComboBoxItem;
                itemSelectedontent = itemSelected.Content as string;
            });
            return itemSelectedontent;
        }

        public void Click(object sender, MouseButtonEventArgs e, Action action) => clickHelper.Click(sender, e, action);
        public void SetTimesOfClick(object sender, MouseButtonEventArgs e) => clickHelper.MouseDown(sender, e);

        protected virtual void SourceManager_ProcessingProgress(object sender, Core.CustomEventArgs.ProcessingEventArgs ev) {
            Task.Run(() => {
                if (notification != null) {
                    dispatcher.Invoke(() => {
                        notification.Message = ev.StateMessage;
                    });
                }
            });
        }
    }
}
