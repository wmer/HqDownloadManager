using ADO.ORM.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HqDownloadManager.Models {
    public class DownloadItem : INotifyPropertyChanged {        
        private int _id;
        [PrimaryKey]
        public int Id {
            get { return _id; }
            set { _id = value;
                OnPropertyChanged("Id");
            }
        }
            
        private byte[] _hq;

        public byte[] Hq {
            get { return _hq; }
            set { _hq = value;
                OnPropertyChanged("Hq");
            }
        }

        private string _downloadLocation;

        public string DownloadLocation {
            get { return _downloadLocation; }
            set { _downloadLocation = value;
                OnPropertyChanged("DownloadLocation");
            }
        }

        private bool _isDownloaded;

        public bool IsDownloaded {
            get { return _isDownloaded; }
            set { _isDownloaded = value;
                OnPropertyChanged("IsDownloaded");
            }
        }

        private DateTime _downloadStarted;

        public DateTime DownloadStarted {
            get { return _downloadStarted; }
            set { _downloadStarted = value;
                OnPropertyChanged("DownloadStarted");
            }
        }

        private DateTime _downloadFinished;

        public DateTime DownloadFinished {
            get { return _downloadFinished; }
            set { _downloadFinished = value;
                OnPropertyChanged("DownloadFinished");
            }
        }

        private int _numChapters;

        public int NumChapters {
            get { return _numChapters; }
            set { _numChapters = value;
                OnPropertyChanged("NumChapters");
            }
        }

        private int _numPages;

        public int NumPages {
            get { return _numPages; }
            set { _numPages = value;
                OnPropertyChanged("NumPages");
            }
        }

        private int _actualChapterindex;

        public int ActualChapterIndex {
            get { return _actualChapterindex; }
            set { _actualChapterindex = value;
                OnPropertyChanged("ActualChapterIndex");
            }
        }

        private int _actualPageIndex;

        public int ActualPageIndex {
            get { return _actualPageIndex; }
            set { _actualPageIndex = value;
                OnPropertyChanged("ActualPageIndex");
            }
        }

        public DownloadItem() {
            ActualChapterIndex = -1;
            ActualPageIndex = -1;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) =>
               PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
