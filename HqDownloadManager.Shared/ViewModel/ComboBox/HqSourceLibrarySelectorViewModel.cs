using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.ViewModel.ComboBox {
    public class HqSourceLibrarySelectorViewModel : ViewModelBase {
        private string[] _sources;
        private string _selectedSource;

        public HqSourceLibrarySelectorViewModel() {
            Sources = new string[]{
                "MangaHost", "YesMangas",
                "UnionMangas", "MangasProject", "MangaLivre",
                "Hipercool", "HqUltimate"
            };
            SelectedSource = Sources[0];
        }

        public string SelectedSource {
            get => _selectedSource;
            set { _selectedSource = value;
                OnPropertyChanged("SelectedSource");
            }
        }


        public string[] Sources {
            get => _sources;
            set { _sources = value;
                OnPropertyChanged("Sources");
            }
        }

    }
}
