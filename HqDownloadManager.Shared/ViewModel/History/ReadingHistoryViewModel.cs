using HqDownloadManager.Shared.Models;
using HqManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.ViewModel.History {
    public class ReadingHistoryViewModel : ViewModelBase {
        private ObservableCollection<ReadingHistory> _readings;
        private ReadingHistory _selectedReading;

        public ReadingHistory SelectedReading {
            get => _selectedReading;
            set {
                _selectedReading = value;
                OnPropertyChanged("SelectedReading");
            }
        }

        public ObservableCollection<ReadingHistory> Readings {
            get => _readings;
            set {
                _readings = value;
                OnPropertyChanged("Readings");
            }
        }
    }
}
