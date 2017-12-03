using HqDownloadManager.Controller.ViewModel;
using HqDownloadManager.Core.Models;
using Repository.Attributes;
using System;
using Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.Models {
    public class DownloadListItem : ViewModelBase, IComparable<DownloadListItem> {
        private Hq _hq;
        private Chapter _downloadChapter;
        private int _totalChapters;
        private int _actualChapter;
        public int _totalPages;
        public int _actualPage;
        public DateTime _startedIn;
        public DateTime _finishedIn;
        public string _status;

        public Hq Hq {
            get => _hq;
            set {
                _hq = value;
                OnPropertyChanged("Hq");
            }
        }

        public Chapter DownloadChapter {
            get => _downloadChapter;
            set {
                _downloadChapter = value;
                OnPropertyChanged("DownloadChapter");
            }
        }

        public int TotalChapters {
            get => _totalChapters;
            set {
                _totalChapters = value;
                OnPropertyChanged("TotalChapters");
            }
        }

        public int ActualChapter {
            get => _actualChapter;
            set {
                _actualChapter = value;
                OnPropertyChanged("ActualChapter");
            }
        }

        public int TotalPages {
            get => _totalPages;
            set {
                _totalPages = value;
                OnPropertyChanged("TotalPages");
            }
        }

        public int ActualPage {
            get => _actualPage;
            set {
                _actualPage = value;
                OnPropertyChanged("ActualPage");
            }
        }

        public DateTime StartedIn {
            get => _startedIn;
            set {
                _startedIn = value;
                OnPropertyChanged("StartedIn");
            }
        }

        public DateTime FinishedIn {
            get => _finishedIn;
            set {
                _finishedIn = value;
                OnPropertyChanged("FinishedIn");
            }
        }

        public string Status {
            get => _status;
            set {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public int CompareTo(DownloadListItem other) {
            return String.Compare(Hq.Title, other.Hq.Title, StringComparison.Ordinal);
        }

        public override bool Equals(object obj) {
            if (obj.GetType() == typeof(DownloadListItem)) {
                var model = obj as DownloadListItem;
                return Hq.Link == model.Hq.Link || (Hq.Title == model.Hq.Title && Hq.Author == model.Hq.Author);
            }
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
