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
        private readonly Window _mainWindow;
        private readonly Dispatcher _dispatcher;

        public ControlsHelper() {
            _mainWindow = Application.Current.MainWindow;
            _dispatcher = _mainWindow.Dispatcher;
        }

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
            _dispatcher.Invoke(() => {
                try {
                    if (_mainWindow.FindResource(key) is T rs) {
                        resource = rs;
                    } else if (GetCurrentPage() is Page page) {
                        resource = (T)page.FindResource(key);
                    }
                } catch (Exception) {
                    if (GetCurrentPage() is Page page) {
                        resource = (T)page.FindResource(key);
                    }
                }
            });

            return resource;
        }

        private T FindInWindow<T>(string name) where T : DependencyObject {
            T element = null;
            _dispatcher.Invoke(() => {
                element = _mainWindow.FindName(name) as T;
            });
            return element;
        }

        private T FindInPage<T>(string name) where T : DependencyObject {
            T element = null;
            _dispatcher.Invoke(() => {
                if (GetCurrentPage() is Page page) {
                    element = page.FindName(name) as T;
                }
            });
            return element;
        }

        public Page GetCurrentPage() {
            Page page = null;
            _dispatcher.Invoke(() => {
                if (FindInWindow<DockPanel>("Content").Children[0] is Frame frame) {
                    page = frame.Content as Page;
                }
            });
            return page;
        }
    }
}
