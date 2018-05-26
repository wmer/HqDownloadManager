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
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.ViewModels {
    public class SourceUpdateViewModel : ViewModelBase {
        private SourceManager _sourceManager;
        private OpenDetailsCommand _openDetailsCommand;
        private GetDetailsCommand _getDetailsCommand;
        private AddToDownloadListCommand _addToDownloadList;

        private ObservableCollection<Update> _updates;
        private Update _selectedUpdate;
        private Chapter _selectedChapter;
        private int _selectedIndex;
        private int _columns;

        public SourceUpdateViewModel(
                        GetDetailsCommand getDetailsCommand, 
                        AddToDownloadListCommand addToDownloadList) {
            var injectionResolver = new DependencyInjection();
            _openDetailsCommand = new OpenDetailsCommand();
            _getDetailsCommand = getDetailsCommand;
            _addToDownloadList = addToDownloadList;
            _sourceManager = injectionResolver.Resolve<SourceManager>();
            var mangaHostSource = _sourceManager.GetSpurce(SourcesEnum.MangaHost);
            Task.Run(() => {
                mangaHostSource.GetUpdates(out List<Update> updates);
                Updates = new ObservableCollection<Update>(updates);
            });
        }

        public int Columns {
            get => _columns;
            set {
                _columns = value;
                OnPropertyChanged("Columns");
            }
        }

        public Update SelectedUpdate {
            get => _selectedUpdate;
            set {
                _selectedUpdate = value;
                OnPropertyChanged("SelectedUpdate");
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
