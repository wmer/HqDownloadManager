using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace HqDownloadManager.Helpers {
    public class NotificationHelper {
        public void Notify(String title, String message) {
            Notification(title, message);
        }

        private void Notification(String title, String message) {
            try {
                var notification = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                var text = notification.GetElementsByTagName("text");
                text[0].AppendChild(notification.CreateTextNode(title));
                text[1].AppendChild(notification.CreateTextNode(message));
                var toast = new ToastNotification(notification);
                ToastNotificationManager.CreateToastNotifier("HQDownloadManager").Show(toast);
            } catch {

            }
        }
    } 
}
