using DependencyInjectionResolver;
using HqDownloadManager.WPF.Commands;
using MangaScraping;
using MangaScraping.Enumerators;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;

namespace HqDownloadManager.WPF.ViewModels {
    public class SourceLibraryViewModel : ViewModelBase {
        private SourceManager _sourceManager;
        private InfiniteScrollCommand _infiniteScrollCommand;
        private OpenDetailsCommand _openDetailsCommand;
        private GetDetailsCommand _getDetailsCommand;
        private AddToDownloadListCommand _addToDownloadList;

        private ObservableCollection<Hq> _hqLibrary;
        private Dictionary<string, string> _lethers;
        private Hq _selectedHq;
        private int _columns;

        public SourceLibraryViewModel(
                    InfiniteScrollCommand infiniteScrollCommand, 
                    OpenDetailsCommand openDetailsCommand, 
                    AddToDownloadListCommand addToDownloadList, 
                    GetDetailsCommand getDetailsCommand, 
                    SourceManager sourceManager) {
            _infiniteScrollCommand = infiniteScrollCommand;
            _openDetailsCommand = openDetailsCommand;
            _addToDownloadList = addToDownloadList;
            _getDetailsCommand = getDetailsCommand;
            _sourceManager = sourceManager;
            var mangaHostSource = _sourceManager.GetSpurce(SourcesEnum.MangaHost);
            Task.Run(() => {
                mangaHostSource.GetLibrary(out List<Hq> library);
                HqLibrary = new ObservableCollection<Hq>(library);
            });
        }

        public int Columns {
            get => _columns;
            set {
                _columns = value;
                OnPropertyChanged("Columns");
            }
        }

        public Hq SelectedHq {
            get => _selectedHq;
            set {
                _selectedHq = value;
                OnPropertyChanged("SelectedHq");
            }
        }

        public ObservableCollection<Hq> HqLibrary {
            get => _hqLibrary;
            set {
                _hqLibrary = value;
                OnPropertyChanged("HqLibrary");
            }
        }
        public Dictionary<string, string> Lethers {
            get => _lethers;
            set {
                _lethers = value;
                OnPropertyChanged("Lethers");
            }
        }

        public DelegateCommand<Dictionary<string, object>> InfiniteScroll {
            get { return _infiniteScrollCommand.Command; }
        }

        public DelegateCommand<DetailsViewModel> OpenDetails {
            get { return _openDetailsCommand.Command; }
        }

        public DelegateCommand<Dictionary<string, object>> GetDetails {
            get { return _getDetailsCommand.Command; }
        }

        public DelegateCommand<Hq> AddToDownload {
            get { return _addToDownloadList.Command; }
        }
    }
}
