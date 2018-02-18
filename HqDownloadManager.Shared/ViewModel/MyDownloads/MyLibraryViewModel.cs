using HqDownloadManager.Download.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.ViewModel.MyDownloads {
    public class MyLibraryViewModel : ViewModelBase { 
        private HqDownloadInfo _selectedDownloadinfo;
        private ObservableCollection<HqDownloadInfo> _downloadInfos;
        private double _width;
        private double _height; private int _columns;

        public int Columns {
            get => _columns;
            set {
                _columns = value;
                OnPropertyChanged("Columns");
            }
        }

        public ObservableCollection<HqDownloadInfo> DownloadInfos {
            get => _downloadInfos;
            set { _downloadInfos = value;
                OnPropertyChanged("DownloadInfos");
            }
        }

        public HqDownloadInfo SelectedDownloadInfo {
            get => _selectedDownloadinfo;
            set {
                _selectedDownloadinfo = value;
                OnPropertyChanged("SelectedDownloadInfo");
            }
        }

        public double Width {
            get => _width;
            set {
                _width = value;
                OnPropertyChanged("Width");
            }
        }

        public double Height {
            get => _height;
            set {
                _height = value;
                OnPropertyChanged("Height");
            }
        }
    }
}
