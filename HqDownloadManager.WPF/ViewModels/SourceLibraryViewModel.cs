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
using WPF.Tools.Commands;

namespace HqDownloadManager.WPF.ViewModels {
    public class SourceLibraryViewModel : ViewModelBase {
        private SourceManager _sourceManager;
        private InfiniteScrollCommand _infiniteScrollCommand;

        private ObservableCollection<Hq> _hqLibrary;
        private Dictionary<string, string> _lethers;
        private Hq _selectedHq;
        private double _width;
        private double _height;
        private int _columns;

        public SourceLibraryViewModel() {
            var injectionResolver = new DependencyInjection();
            _infiniteScrollCommand = new InfiniteScrollCommand();
            _sourceManager = injectionResolver.Resolve<SourceManager>();
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

        public double Width {
            get => _width;
            set {
                _width = value;
                OnPropertyChanged("Width");
            }
        }
        public double Height {
            get => _height;
            set {
                _height = value;
                OnPropertyChanged("Height");
            }
        }

        public DelegateCommand<Dictionary<string, object>> InfiniteScroll {
            get { return _infiniteScrollCommand.Command; }
        }
    }
}
