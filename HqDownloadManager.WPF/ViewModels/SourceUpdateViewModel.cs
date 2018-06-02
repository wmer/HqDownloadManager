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
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.ViewModels {
    public class SourceUpdateViewModel : ViewModelBase {
        private Dictionary<string, IHqSourceManager> _hqsources;
        private SourceManager _sourceManager;
        private OpenDetailsCommand _openDetailsCommand;
        private GetDetailsCommand _getDetailsCommand;
        private AddToDownloadListCommand _addToDownloadList;

        private ObservableCollection<Update> _updates;
        private Update _selectedUpdate;
        private Chapter _selectedChapter;
        private int _selectedIndex;
        private string[] _sources;
        private string _selectedSource;

        public SourceUpdateViewModel(
                        SourceManager sourceManager,
                        GetDetailsCommand getDetailsCommand, 
                        AddToDownloadListCommand addToDownloadList) {
            _openDetailsCommand = new OpenDetailsCommand();
            _getDetailsCommand = getDetailsCommand;
            _addToDownloadList = addToDownloadList;
            _sourceManager = sourceManager;
            _hqsources = _sourceManager.GetSources();
            Sources = new string[]{
                "MangaHost", "YesMangas",
                "UnionMangas", "MangasProject",
                "MangaLivre"
            };
            SelectedSource = Sources[0];
        }

        public Update SelectedUpdate {
            get => _selectedUpdate;
            set {
                _selectedUpdate = value;
                OnPropertyChanged("SelectedUpdate");
                _addToDownloadList.RaiseCanExecuteChanged();
            }
        }

        public Chapter SelectedChapter {
            get => _selectedChapter;
            set {
                _selectedChapter = value;
                OnPropertyChanged("SelectedChapter");
            }
        }

        public int SelectedIndex {
            get => _selectedIndex;
            set {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public ObservableCollection<Update> Updates {
            get => _updates;
            set {
                _updates = value;
                OnPropertyChanged("Updates");
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
                hqSource.GetUpdates(out List<Update> updates);
                Updates = new ObservableCollection<Update>(updates);
            });
        }
    }
}
