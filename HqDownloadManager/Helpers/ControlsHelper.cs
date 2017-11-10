using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HqDownloadManager.Helpers {
    public class ControlsHelper {

        public T Find<T>(string name) where T : UIElement {
            T element = null;
            if (FindInMainPage<T>(name) is T elW) {
                element = elW;
            } else if (FindInCurrentPage<T>(name) is T elP) {
                element = elP;
            }
            return element;
        }

        public T FindResource<T>(string key) {
            T resource = default(T);
            try {
                if (GetMainPage().Resources[key] is T rs) {
                    resource = rs;
                } else if (GetCurrentPage() is Page page) {
                    resource = (T)page.Resources[key];
                }
            } catch (Exception) {
                try {
                    if (GetCurrentPage() is Page page) {
                        resource = (T)page.Resources[key];
                    }
                } catch {
                    // ignored
                }
            }

            return resource;
        }

        private T FindInMainPage<T>(string name) where T : UIElement {
            T element = null;
            element = GetMainPage().FindName(name) as T;
            return element;
        }

        private T FindInCurrentPage<T>(string name) where T : UIElement {
            T element = null;
            if (GetCurrentPage() is Page page) {
                element = page.FindName(name) as T;
            }
            return element;
        }

        private Page GetMainPage() {
            return Window.Current?.Content as Page;
        }

        public Page GetCurrentPage() {
            Page page = null;
            if (FindInMainPage<NavigationView>("Navigation").Content is Frame frame) {
                page = frame.Content as Page;
            }
            return page;
        }
    }
}
