using HqDownloadManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HqDownloadManager.Helpers {
    public class ControlsHelper {
        private readonly Window _mainWindow = Application.Current.MainWindow;

        public T Find<T>(string name) where T : DependencyObject {
            T element = null;
            if (FindInWindow<T>(name) != null) {
                element = FindInWindow<T>(name);
            } else if (GetCurrentPage() != null) {
                element = GetCurrentPage().FindName(name) as T;
            }

            return element;
        }

        public T FindResource<T>(string key) {
            T resource = default(T);
            if (_mainWindow.FindResource(key) != null) {
                resource = (T) _mainWindow.FindResource(key);
            }else if (GetCurrentPage() != null) {
                resource = (T) GetCurrentPage().FindResource(key);
            }

            return resource;
        }

        private T FindInWindow<T>(string name) where T : DependencyObject {
            T element = null;
            element = _mainWindow.FindName(name) as T;
            return element;
        }

        public Page GetCurrentPage() {
            var frame = FindInWindow<DockPanel>("Content").Children[0] as Frame;
            return frame.Content as Page;
        }
    }
}
