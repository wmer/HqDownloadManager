﻿using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.ViewModel;
using WPF.Tools.Navigation.Events;

namespace HqDownloadManager.WPF.ViewModels
{
    public class ReaderViewModel : ViewModelBase {
        private ObservableCollection<Page> _pages;

        public ObservableCollection<Page> Pages {
            get { return _pages; }
            set {
                _pages = value;
                OnPropertyChanged("Pages");
            }
        }

        private Page _actualPage;

        public Page ActualPage {
            get { return _actualPage; }
            set {
                _actualPage = value;
                OnPropertyChanged("ActualPage");
            }
        }


        public ReaderViewModel() {
            NavigationEventHub.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e) {
            var pages = e.ExtraContent[2];
            Pages = new ObservableCollection<Page>(pages as List<Page>);
        }
    }
}
