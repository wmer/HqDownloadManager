using DependencyInjectionResolver;
using HqDownloadManager.WPF.Commands;
using MangaScraping;
using MangaScraping.Enumerators;
using MangaScraping.Managers;
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
        private Dictionary<string, IHqSourceManager> _hqsources;
        private SourceManager _sourceManager;
        private InfiniteScrollCommand _infiniteScrollCommand;
        private OpenDetailsCommand _openDetailsCommand;
        private GetDetailsCommand _getDetailsCommand;
        private AddToDownloadListCommand _addToDownloadList;

        private ObservableCollection<Hq> _hqLibrary;
        private Dictionary<string, string> _lethers;
        private Hq _selectedHq;
        private string[] _sources;
        private string _selectedSource;

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
            _hqsources = _sourceManager.GetSources();
            Sources = new string[]{
                "MangaHost", "YesMangas",
                "UnionMangas", "MangasProject",
                "MangaLivre"
            };
            SelectedSource = Sources[0];
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

        public string SelectedSource {
            get => _selectedSource;
            set {
                _selectedSource = value;
                OnPropertyChanged("SelectedSource");
                GetSource(_selectedSource);
            }
        }


        public string[] Sources {
            get => _sources;
            set {
                _sources = value;
                OnPropertyChanged("Sources");
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

        private void GetSource(string source) {
            var hqSource = _hqsources[source];
            Task.Run(() => {
                hqSource.GetLibrary(out List<Hq> library);
                HqLibrary = new ObservableCollection<Hq>(library);
            });
        }
    }
}
