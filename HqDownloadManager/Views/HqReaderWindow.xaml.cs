using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DependencyInjectionResolver;
using HqDownloadManager.Controllers;
using HqDownloadManager.Core;
using HqDownloadManager.Core.Models;
using HqDownloadManager.ViewModels;

namespace HqDownloadManager.Views {
    public partial class HqReaderWindow : Window {
        private readonly DependencyInjection _dependency;
        private HqReaderController _hqReaderController;

        private Hq _hq;
        private Chapter _chapter;
        public HqReaderWindow(Hq hq, DependencyInjection dependency) {
            _hq = hq;
            _dependency = dependency;
            InitializeComponent();
            Loaded += OnLoaded;
            Closed += OnClosed;
        }

        public HqReaderWindow(Chapter chapter, DependencyInjection dependency) {
            _chapter = chapter;
            _dependency = dependency;
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
            _hqReaderController = _dependency.Resolve<HqReaderController>();
            if (_hq != null) {
                _hqReaderController.Init(_hq);
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e) {
            _hqReaderController.PreviousChapter();
        }

        private void Next_Click(object sender, RoutedEventArgs e) {
            _hqReaderController.NextChapter();
        }

        private void HqReaderWindow_OnKeyDown(object sender, KeyEventArgs e) {
            if (Keyboard.IsKeyDown(Key.Left)) {
                _hqReaderController.PreviousPage();
            }
            if (Keyboard.IsKeyDown(Key.Right)) {
                _hqReaderController.NextPage();
            }
        }

        private void OnClosed(object sender, EventArgs eventArgs) {
            var window = _dependency.Resolve<MainWindow>();
            Application.Current.MainWindow = window;
        }
    }
}
