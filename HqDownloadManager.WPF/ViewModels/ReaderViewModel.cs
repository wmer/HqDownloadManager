using HqDownloadManager.WPF.Databases;
using HqDownloadManager.WPF.Models;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Tools.MVVM.ViewModel;
using WPF.Tools.Navigation.Events;

namespace HqDownloadManager.WPF.ViewModels {
    public class ReaderViewModel : ViewModelBase {
        private ObservableCollection<Page> _pages;

        public ObservableCollection<Page> Pages {
            get { return _pages; }
            set { _pages = value;
                OnPropertyChanged("Pages");
            }
        }


        public ReaderViewModel() {
            NavigationEventHub.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e) {
            var pages = e.ExtraContent[0] as object[];
            Pages = new ObservableCollection<Page>(pages[0] as List<Page>);
        }
    }
}
