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
using WPF.Tools.MVVM.Commands;
using WPF.Tools.MVVM.ViewModel;
using WPF.Tools.Navigation;
using WPF.Tools.Navigation.Events;

namespace HqDownloadManager.WPF.ViewModels {
    public class SourceUpdateViewModel : ViewModelBase {
        private Dictionary<string, IHqSourceManager> _hqsources;
        private SourceManager _sourceManager;
        private OpenDetailsCommand _openDetailsCommand;
        private GetDetailsCommand _getDetailsCommand;
        private bool started;

        private ObservableCollection<Update> _updates;
        private Update _selectedUpdate;
        private string[] _sources;
        private string _selectedSource;

        public SourceUpdateViewModel(
                        SourceManager sourceManager,
                        OpenDetailsCommand openDetailsCommand,
                        GetDetailsCommand getDetailsCommand) {
            _sourceManager = sourceManager;
            _openDetailsCommand = openDetailsCommand;
            _getDetailsCommand = getDetailsCommand;
            _hqsources = _sourceManager.GetSources();
            Sources = new string[]{
                "MangaHost", "YesMangas",
                "MangasProject", "MangaLivre"
            };
            started = false;
            SelectedSource = Sources[0];
            NavigationEventHub.Navigated += OnNavigated;
        }

        public Update SelectedUpdate {
            get => _selectedUpdate;
            set {
                _selectedUpdate = value;
                OnPropertyChanged("SelectedUpdate");
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
            var hqSource = _hqsources[SelectedSource];
            Task.Run(() => {
                hqSource.GetUpdates(out List<Update> updates);
                Updates = new ObservableCollection<Update>(updates);
            });
        }
    }
}
