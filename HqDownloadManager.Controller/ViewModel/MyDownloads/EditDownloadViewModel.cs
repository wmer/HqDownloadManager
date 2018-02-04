using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace HqDownloadManager.Controller.ViewModel.MyDownloads {
    public class EditDownloadViewModel : ViewModelBase {
        private Hq _hq;
        private Visibility _visibility;
        private Visibility _resultVisibility;
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

        public Visibility Visibility {
            get => _visibility;
            set {
                _visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

        public Visibility ResultVisibility {
            get => _resultVisibility;
            set {
                _resultVisibility = value;
                OnPropertyChanged("ResultVisibility");
            }
        }
    }
}
