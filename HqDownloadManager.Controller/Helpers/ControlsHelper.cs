using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HqDownloadManager.Controller.Helpers {
    public class ControlsHelper {
        public T Find<T>(string name) where T : UIElement {
            T element = null;
            if (FindInMainPage<T>(name) is T elW) {
                element = elW;
            } else if (FindInCurrentPage<T>(name) is T elP) {
                element = elP;
            } else if (FindChildControl<T>(GetCurrentPage(), name) is T elC) {
                element = elC;
            } else if (FindChildControl<T>(GetMainPage(), name) is T elCM) {
                element = elCM;
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

        public T FindIn<T>(DependencyObject control, string name) where T: DependencyObject {
            T element = default(T);
            try {
                if (FindChildControl<T>(control, name) is T elC) {
                    element = elC;
                }
            } catch {
                // ignored
            }
            return element;
        }

        public List<T> FindIn<T>(DependencyObject control) where T : DependencyObject {
            var list = new List<T>();
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++) {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);

                if (child is T c) {
                    list.Add(c);
                } else {
                    list.AddRange(FindIn<T>(child));
                }

            }

            return list;
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

        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName) {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++) {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                if (fe == null) return null;
                if (fe.Name == ctrlName) {
                    return child;
                }

                if (child is T && fe.Name == ctrlName) {
                    return child;
                } else {
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }
    }
}
