using HqDownloadManager.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;
using WPF.Tools.Navigation.Events;

namespace HqDownloadManager.WPF.ViewModels {
    public class MainWindowViewModel : ViewModelBase {
        private FullScreenCommand _fullScreenCommand;
        private ExitFullScreenCommand _exitFullScreenCommand;

        public MainWindowViewModel(
                    FullScreenCommand fullScreenCommand,
                    ExitFullScreenCommand exitFullScreenCommand) {
            _fullScreenCommand = fullScreenCommand;
            _exitFullScreenCommand = exitFullScreenCommand;

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

        public DelegateCommand<KeyEventArgs> FullScreen { get => _fullScreenCommand.Command; }
        public DelegateCommand<KeyEventArgs> ExitFullScreen { get => _exitFullScreenCommand.Command; }

        private void OnNavigated(object sender, global::WPF.Tools.Navigation.Events.NavigationEventArgs e) {
           PageTitle = e.Title;
        }
    }
}
