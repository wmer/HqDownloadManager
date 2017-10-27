using HqDownloadManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HqDownloadManager.Helpers {
    public class ControlsHelper {
        public Window Window => Application.Current.MainWindow;
        public Dispatcher Dispacher => Application.Current.MainWindow?.Dispatcher;


        public T Find<T>(string name) where T : DependencyObject {
            T element = null;
            if (FindInWindow<T>(name) is T elW) {
                element = elW;
            } else if (FindInPage<T>(name) is T elP) {
                element = elP;
            }
            return element;
        }

        public T FindResource<T>(string key) {
            T resource = default(T);
            Dispacher.Invoke(() => {
                try {
                    if (Window.FindResource(key) is T rs) {
                        resource = rs;
                    } else if (GetCurrentPage() is Page page) {
                        resource = (T)page.FindResource(key);
                    }
                } catch (Exception) {
                    try {
                        if (GetCurrentPage() is Page page) {
                            resource = (T)page.FindResource(key);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            });

            return resource;
        }

        private T FindInWindow<T>(string name) where T : DependencyObject {
            T element = null;
            Dispacher.Invoke(() => {
                element = Window.FindName(name) as T;
            });
            return element;
        }

        private T FindInPage<T>(string name) where T : DependencyObject {
            T element = null;
            Dispacher.Invoke(() => {
                if (GetCurrentPage() is Page page) {
                    element = page.FindName(name) as T;
                }
            });
            return element;
        }

        public Page GetCurrentPage() {
            Page page = null;
            Dispacher.Invoke(() => {
                if (FindInWindow<DockPanel>("Content").Children[0] is Frame frame) {
                    page = frame.Content as Page;
                }
            });
            return page;
        }
    }
}
