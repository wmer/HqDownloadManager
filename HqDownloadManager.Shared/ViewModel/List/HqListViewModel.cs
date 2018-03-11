using HqDownloadManager.Core.Models;
using HqManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.ViewModel.List {
    public class HqListViewModel : ViewModelBase {
        private ObservableCollection<HqEntry> _entries;
        private HqEntry _selectedEntry;
        private bool _showDetails;
        private List<string> _status;
        private string _selectedStatus;
        private List<double> _scores;
        private double _score;
        private string _review;
        private List<Chapter> _updates;

        public HqListViewModel() {
            Status = Enum.GetNames(typeof(ReadStatus)).ToList();
            Status.Add("Add to...");
            SelectedStatus = Status.Last();
            Scores = new List<double> {
                0, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8, 8.5, 9, 9.5, 10
            };
        }

        public bool ShowDetails {
            get => _showDetails;
            set {
                _showDetails = value;
                OnPropertyChanged("ShowDetails");
            }
        }

        public HqEntry SelectedEntry {
            get => _selectedEntry;
            set {
                _selectedEntry = value;
                SelectedStatus = _selectedEntry.ReadStatus;
                Score = _selectedEntry.Score;
                Review = _selectedEntry.Review;
                OnPropertyChanged("SelectedEntry");
            }
        }

        public ObservableCollection<HqEntry> Entries {
            get => _entries;
            set {
                _entries = value;
                OnPropertyChanged("Entries");
            }
        }

        public List<string> Status {
            get => _status;
            set {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public string SelectedStatus {
            get => _selectedStatus;
            set {
                _selectedStatus = value;
                OnPropertyChanged("SelectedStatus");
            }
        }

        public List<double> Scores {
            get => _scores;
            set {
                _scores = value;
                OnPropertyChanged("Scores");
            }
        }

        public double Score {
            get => _score;
            set {
                _score = value;
                OnPropertyChanged("Score");
            }
        }

        public string Review {
            get => _review;
            set {
                _review = value;
                OnPropertyChanged("Review");
            }
        }

        public List<Chapter> Updates {
            get => _updates;
            set { _updates = value;
                OnPropertyChanged("Updates");
            }
        }

        public ReadStatus ReadStatus {
            get => (ReadStatus)Enum.Parse(typeof(ReadStatus), SelectedStatus);
        }

        public HqEntry Entry {
            get {
                return new HqEntry {
                    Hq = SelectedEntry.Hq,
                    ReadStatus = SelectedStatus,
                    Score = Score,
                    LastChapterRead = SelectedEntry.LastChapterRead,
                    Review = Review,
                    StartedReading = SelectedEntry.StartedReading,
                    FinishedReading = SelectedEntry.FinishedReading
                };
            }
            set {
                SelectedEntry = value;
            }
        }
    }
}
