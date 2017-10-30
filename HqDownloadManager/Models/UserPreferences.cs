using HqDownloadManager.ViewModels;
using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Models {
    public class UserPreferences : ViewModelBase {
        private int _id;
        private bool _compress;
        private bool _eraseFolder;
        private string _downloadPath;
        private bool _shutdown;


        [PrimaryKey]
        public int Id {
            get => _id;
            set {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string DownloadPath {
            get => _downloadPath;
            set {
                _downloadPath = value;
                OnPropertyChanged("DownloadPath");
            }
        }


        public bool EraseFolder {
            get => _eraseFolder;
            set {
                _eraseFolder = value;
                OnPropertyChanged("EraseFolder");
            }
        }


        public bool Compress {
            get => _compress;
            set {
                _compress = value;
                OnPropertyChanged("Compress");
            }
        }

        public bool Shutdown {
            get => _shutdown;
            set {
                _shutdown = value;
                OnPropertyChanged("Shutdown");
            }
        }
    }
}
