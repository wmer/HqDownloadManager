using HqDownloadManager.Models;
using HqDownloadManager.WPF.UserControls.Commands;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;
using WPF.Tools.Navigation.Events;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.UserControls.ViewModels {
    public class DetailsViewModel : ViewModelBase {
        private CloseDetailsCommand _closeDetailsCommand;
        private AddToDownloadListCommand _addToDownloadList;
        private AddToDownloadListSelectedChapters _addToDownloadListSelectedChapters;
        private AddUpdatesToDownloadCommand _addUpdatesToDownloadCommand;
        private OpenOnlineReaderCommand _openOnlineReaderCommand;

        public DetailsViewModel(
                   CloseDetailsCommand closeDetailsCommand,
                   AddToDownloadListCommand addToDownloadList,
                   AddToDownloadListSelectedChapters addToDownloadListSelectedChapters,
                   AddUpdatesToDownloadCommand addUpdatesToDownloadCommand,
                   OpenOnlineReaderCommand openOnlineReaderCommand) {
            _closeDetailsCommand = closeDetailsCommand;
            _addToDownloadList = addToDownloadList;
            _addToDownloadListSelectedChapters = addToDownloadListSelectedChapters;
            _addUpdatesToDownloadCommand = addUpdatesToDownloadCommand;
            _openOnlineReaderCommand = openOnlineReaderCommand;
        }



        public DelegateCommand<DetailsUserControl> CloseDetails {
            get => _closeDetailsCommand.Command;
        }

        public DelegateCommand<Hq> AddToDownload {
            get => _addToDownloadList.Command;
        }

        public DelegateCommand<DetailsUserControl> DownloadSelected {
            get => _addToDownloadListSelectedChapters.Command;
        }

        public DelegateCommand<DetailsUserControl> DownloadUpdates {
            get => _addUpdatesToDownloadCommand.Command;
        }

        public DelegateCommand<DetailsUserControl> OpenReader {
            get => _openOnlineReaderCommand.Command;
        }
    }
}
