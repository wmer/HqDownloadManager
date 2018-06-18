using HqDownloadManager.WPF.Commands;
using HqDownloadManager.WPF.UserControls;
using MangaScraping;
using MangaScraping.Managers;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;
using WPF.Tools.Navigation.Events;

namespace HqDownloadManager.WPF.ViewModels {
    public class SourceLibraryViewModel : ViewModelBase {
        private Dictionary<string, IHqSourceManager> _hqsources;
        private SourceManager _sourceManager;
        private OpenDetailsCommand _openDetailsCommand;
        private GetDetailsCommand _getDetailsCommand;
        private InfiniteScrollCommand _infiniteScrollCommand;
        private GetFinalizedCommand _getFinalizedCommand;
        private ShowLetherCommand _showLetherCommand;
        private bool started;

        private ObservableCollection<Hq> _hqLibrary;
        private ObservableCollection<string> _lethers;
        private Hq _selectedHq;
        private string[] _sources;
        private string _selectedSource;
        private string _selectedLether;

        public SourceLibraryViewModel(
                        SourceManager sourceManager,
                        OpenDetailsCommand openDetailsCommand,
                        GetDetailsCommand getDetailsCommand,
                        InfiniteScrollCommand infiniteScrollCommand,
                        GetFinalizedCommand getFinalizedCommand,
                        ShowLetherCommand showLetherCommand) {
            _sourceManager = sourceManager;
            _openDetailsCommand = openDetailsCommand;
            _getDetailsCommand = getDetailsCommand;
            _infiniteScrollCommand = infiniteScrollCommand;
            _getFinalizedCommand = getFinalizedCommand;
            _showLetherCommand = showLetherCommand;
            _hqsources = _sourceManager.GetSources();

            Sources = new string[]{
                "MangaHost", "YesMangas",
                "UnionMangas", "MangasProject",
                "MangaLivre"
            };
            started = false;
            SelectedSource = Sources[0];
            NavigationEventHub.Navigated += OnNavigated;
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
        public ObservableCollection<string> Lethers {
            get => _lethers;
            set {
                _lethers = value;
                OnPropertyChanged("Lethers");
                Application.Current.Dispatcher.Invoke(()=> {
                    _getFinalizedCommand.RaiseCanExecuteChanged();
                });
            }
        }

        public string SelectedLether {
            get { return _selectedLether; }
            set { _selectedLether = value;
                OnPropertyChanged("SelectedLether");
            }
        }

        public string SelectedSource {
            get => _selectedSource;
            set {
                _selectedSource = value;
                OnPropertyChanged("SelectedSource");
                if (started) {
                    ShowMangas(_selectedSource);
                }
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

        public DelegateCommand<SourceLibraryViewModel> GetFinalized {
            get { return _getFinalizedCommand.Command; }
        }

        public DelegateCommand<SourceLibraryViewModel> GetLether {
            get { return _showLetherCommand.Command; }
        }

        public DelegateCommand<DetailsUserControl> OpenDetails {
            get => _openDetailsCommand.Command;
        }

        public DelegateCommand<Tuple<string, object, DetailsUserControl>> GetDetails {
            get => _getDetailsCommand.Command;
        }

        private void OnNavigated(object sender, NavigationEventArgs e) {
            started = true;
            ShowMangas(SelectedSource);
        }

        private void ShowMangas(string source) {
            var hqSource = _hqsources[source];
            Task.Run(() => {
                hqSource.GetLibrary(out List<Hq> library, out List<string> lethers);
                HqLibrary = new ObservableCollection<Hq>(library);
                Lethers = new ObservableCollection<string>(lethers);
            });
        }
    }
}
