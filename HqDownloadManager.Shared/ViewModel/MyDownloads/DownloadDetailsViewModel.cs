using HqDownloadManager.Core.Models;
using HqDownloadManager.Download.Models;
using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HqDownloadManager.Shared.ViewModel.MyDownloads {
    public class DownloadDetailsViewModel : ViewModelBase {
        private Chapter _selectedChapter;
        private HqDownloadInfo _downloadInfo;
        private List<ReaderHistory> _readings;
        private ReaderHistory _selectedReading;
        private List<Chapter> _updateList;
        private bool _updateVisibility;
        private double _width;
        private double _height;
        private int _columns;

        public int Columns {
            get => _columns;
            set {
                _columns = value;
                OnPropertyChanged("Columns");
            }
        }

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

        public bool UpdateVisibility {
            get => _updateVisibility;
            set {
                _updateVisibility = value;
                OnPropertyChanged("UpdateVisibility");
            }
        }

        public List<ReaderHistory> Readings {
            get => _readings;
            set {
                _readings = value;
                OnPropertyChanged("Readings");
            }
        }

        public ReaderHistory SelectedReading {
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
