using HqDownloadManager.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;
using WPF.Tools.Navigation.Events;

namespace HqDownloadManager.WPF.ViewModels {
    public class MainWindowViewModel : ViewModelBase {
        private DragWindowCommand _dragWindowCommand;
        private MinimizeWindowCommand _minimizeWindowCommand;
        private MaximizeWindowCommand _maximizeWindowCommand;
        private FullScreenCommand _fullScreenCommand;
        private CloseWindowCommand _closeWindowCommand;

        public MainWindowViewModel(
                    DragWindowCommand dragWindowCommand,
                    MinimizeWindowCommand minimizeWindowCommand,
                    MaximizeWindowCommand maximizeWindowCommand,
                    FullScreenCommand fullScreenCommand,
                    CloseWindowCommand closeWindowCommand) {
            _dragWindowCommand = dragWindowCommand;
            _minimizeWindowCommand = minimizeWindowCommand;
            _maximizeWindowCommand = maximizeWindowCommand;
            _fullScreenCommand = fullScreenCommand;
            _closeWindowCommand = closeWindowCommand;

            NavigationEventHub.Navigated += OnNavigated;
        }


        private string _pageTitle;

        public string PageTitle {
            get => _pageTitle;
            set {
                _pageTitle = value;
                OnPropertyChanged("PageTitle");
            }
        }

        public DelegateCommand<Window> DragWindow { get => _dragWindowCommand.Command; }
        public DelegateCommand<Window> MinimizeWindow { get => _minimizeWindowCommand.Command; }
        public DelegateCommand<Window> MaximizeWindow { get => _maximizeWindowCommand.Command; }
        public DelegateCommand<Window> FullScreen { get => _fullScreenCommand.Command; }
        public DelegateCommand<Window> CloaseWindow { get => _closeWindowCommand.Command; }

        private void OnNavigated(object sender, global::WPF.Tools.Navigation.Events.NavigationEventArgs e) {
            PageTitle = e.Title;
        }
    }
}
