using ADO.ORM.Attributes;
using HqDownloadManager.WPF.Databases;
using HqDownloadManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Windows;
using WPF.Tools.MVVM.ViewModel;
using WPF.Tools.Navigation.Events;

namespace HqDownloadManager.WPF.Models {
    public class ChapterReadingProgress : ViewModelBase {
        private readonly ReaderContext _readerContext;

        private int _id;

        public int Id {
            get { return _id; }
            set { _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _chapterTitle;

        public string ChapterTitle {
            get { return _chapterTitle; }
            set { _chapterTitle = value;
                OnPropertyChanged("ChapterTitle");
            }
        }

        private string _hqLocation;

        public string HqLocation {
            get { return _hqLocation; }
            set { _hqLocation = value; }
        }


        private string _location;

        public string Location {
            get { return _location; }
            set { _location = value;
                OnPropertyChanged("Location");
            }
        }

        private int _totalPages;

        public int TotalPages {
            get { return _totalPages; }
            set { _totalPages = value;
                OnPropertyChanged("TotalPages");
            }
        }

        private int _numPage;

        public int NumPage {
            get { return _numPage; }
            set { _numPage = value;
                OnPropertyChanged("NumPage");
            }
        }

        private string _page;

        public string Page {
            get { return _page; }
            set { _page = value;
                OnPropertyChanged("Page");
            }
        }

        private DateTime _date;

        public DateTime Date { 
            get { return _date; }
            set { _date = value;
                OnPropertyChanged("Date");
            }
        }

        public ChapterReadingProgress(ReaderContext readerContext) {
            _readerContext = readerContext;
            NavigationEventHub.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e) {
            string messageBoxText = "Você quer continuar de onde parou?";
            string caption = "Leitor";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            if (e.ExtraContent[0] is ChapterReadingProgress readingProgress) {
            }  
        }
    }
}
