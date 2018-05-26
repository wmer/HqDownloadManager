using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.ViewModel;

namespace HqDownloadManager.WPF.ViewModels {
    public class HqStatusViewModel : ViewModelBase {
        private Hq _hq;
        private string _selectedStatus;
        private double _score;
        private string _review;
        private Chapter _lastChapterRead;
        private DateTimeOffset _startedReading;
        private DateTimeOffset _finishedReading;
        private List<string> _status;
        private List<double> _scores;
        private bool _visibility;

        //public HqStatusViewModel() {
        //    Status = Enum.GetNames(typeof(ReadStatus)).ToList();
        //    Status.Add("Add to...");
        //    SelectedStatus = Status.Last();
        //    Scores = new List<double> {
        //        0, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8, 8.5, 9, 9.5, 10
        //    };
        //}

        //public HqEntry Entry {
        //    get {
        //        return new HqEntry {
        //            Hq = Hq,
        //            ReadStatus = SelectedStatus,
        //            Score = Score,
        //            LastChapterRead = LastChapterRead.Title,
        //            Review = Review,
        //            StartedReading = StartedReading.DateTime,
        //            FinishedReading = FinishedReading.DateTime
        //        };
        //    }
        //    set {
        //        var entry = value;
        //        if (entry == null) {
        //            entry = new HqEntry {
        //                ReadStatus = "Add to..."
        //            };
        //        }
        //        Hq = entry.Hq;
        //        SelectedStatus = entry.ReadStatus;
        //        Score = entry.Score;
        //        Review = entry.Review;
        //        LastChapterRead = new Chapter { Title = entry.LastChapterRead };
        //        StartedReading = entry.StartedReading;
        //        FinishedReading = entry.FinishedReading;
        //    }
        //}

        public List<string> Status {
            get => _status;
            set {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public List<double> Scores {
            get => _scores;
            set {
                _scores = value;
                OnPropertyChanged("Scores");
            }
        }

        //public ReadStatus ReadStatus {
        //    get {
        //        return (ReadStatus)Enum.Parse(typeof(ReadStatus), SelectedStatus);
        //    }
        //}

        public Chapter LastChapterRead {
            get { return _lastChapterRead; }
            set {
                _lastChapterRead = value;
                OnPropertyChanged("LastChapterRead");
            }
        }

        public DateTimeOffset FinishedReading {
            get { return _finishedReading; }
            set {
                _finishedReading = value;
                OnPropertyChanged("FinishedReading");
            }
        }


        public DateTimeOffset StartedReading {
            get { return _startedReading; }
            set {
                _startedReading = value;
                OnPropertyChanged("StartedReading");
            }
        }


        public string Review {
            get { return _review; }
            set {
                _review = value;
                OnPropertyChanged("Review");
            }
        }


        public double Score {
            get { return _score; }
            set {
                _score = value;
                OnPropertyChanged("Score");
            }
        }

        public string SelectedStatus {
            get { return _selectedStatus; }
            set {
                _selectedStatus = value;
                OnPropertyChanged("SelectedStatus");
            }
        }

        public Hq Hq {
            get { return _hq; }
            set {
                _hq = value;
                OnPropertyChanged("Hq");
            }
        }

        public bool Visibility {
            get => _visibility;
            set {
                _visibility = value;
                OnPropertyChanged("Visibility");
            }
        }
    }
}
