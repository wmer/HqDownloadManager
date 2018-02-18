using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HqDownloadManager.Shared.ViewModel.MyDownloads {
    public class EditDownloadViewModel : ViewModelBase {
        private Hq _hq;
        private bool _visibility;
        private bool _resultVisibility;
        private List<Hq> _results;
        private string _urlSearch;

        public string UrlSearch {
            get => _urlSearch;
            set { _urlSearch = value;
                OnPropertyChanged("UrlSearch");
            }
        }


        public List<Hq> Results {
            get { return _results; }
            set { _results = value;
                OnPropertyChanged("Results");
            }
        }

        public Hq Hq {
            get => _hq;
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

        public bool ResultVisibility {
            get => _resultVisibility;
            set {
                _resultVisibility = value;
                OnPropertyChanged("ResultVisibility");
            }
        }
    }
}
