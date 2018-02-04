using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Download.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace HqDownloadManager.Controller.ViewModel.MyDownloads {
    public class DownloadDetailsViewModel : ViewModelBase {
        private Chapter _selectedChapter;
        private HqDownloadInfo _downloadInfo;
        private List<ReaderViewModel> _readings;
        private ReaderViewModel _selectedReading;
        private List<Chapter> _updateList;
        private Visibility _updateVisibility;
        private double _width;
        private double _height;

        public HqDownloadInfo DownloadInfo {
            get => _downloadInfo;
            set {
                _downloadInfo = value;
                OnPropertyChanged("DownloadInfo");
            }
        }

        public Chapter SelectedChapter {
            get => _selectedChapter;
            set {
                _selectedChapter = value;
                OnPropertyChanged("SelectedChapter");
            }
        }

        public List<Chapter> UpdateList {
            get => _updateList;
            set {
                _updateList = value;
                OnPropertyChanged("UpdateList");
            }
        }

        public Visibility UpdateVisibility {
            get => _updateVisibility;
            set {
                _updateVisibility = value;
                OnPropertyChanged("UpdateVisibility");
            }
        }

        public List<ReaderViewModel> Readings {
            get => _readings;
            set {
                _readings = value;
                OnPropertyChanged("Readings");
            }
        }

        public ReaderViewModel SelectedReading {
            get => _selectedReading;
            set {
                _selectedReading = value;
                OnPropertyChanged("SelectedReading");
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
