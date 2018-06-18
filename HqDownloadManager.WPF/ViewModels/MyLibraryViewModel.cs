using HqDownloadManager.Helpers;
using HqDownloadManager.Models;
using HqDownloadManager.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;

namespace HqDownloadManager.WPF.ViewModels {
    public class MyLibraryViewModel : ViewModelBase {
        private OpenDownloadDetailsCommand _openDownloadDetailsCommand;
        private readonly DownloadHelper _downloadHelper;

        private DownloadedHq _selectedDownloadinfo;
        private ObservableCollection<DownloadedHq> _downloadInfos;
        private int _columns;

        public MyLibraryViewModel(
                        DownloadHelper downloadHelper,
                        OpenDownloadDetailsCommand openDownloadDetailsCommand) {
            _downloadHelper = downloadHelper;
            _openDownloadDetailsCommand = openDownloadDetailsCommand;
            Task.Run(() => {
                var downloadedsHqs = _downloadHelper.GetDownloads();
                DownloadedHqs = new ObservableCollection<DownloadedHq>(downloadedsHqs);
            });
        }

        public int Columns {
            get => _columns;
            set {
                _columns = value;
                OnPropertyChanged("Columns");
            }
        }

        public ObservableCollection<DownloadedHq> DownloadedHqs {
            get => _downloadInfos;
            set {
                _downloadInfos = value;
                OnPropertyChanged("DownloadedHqs");
            }
        }

        public DownloadedHq DownloadedHqSelected {
            get => _selectedDownloadinfo;
            set {
                _selectedDownloadinfo = value;
                OnPropertyChanged("DownloadedHqSelected");
            }
        }

        public DelegateCommand<DownloadedHq> OpenDetails {
            get { return _openDownloadDetailsCommand.Command; }
        }
    }
}
