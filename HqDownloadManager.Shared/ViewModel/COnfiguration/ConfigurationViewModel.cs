using HqDownloadManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Shared.ViewModel.COnfiguration {
    public class ConfigurationViewModel : ViewModelBase {
        private Configuration _configuration;
        private DownloadLocation _selectedPath;

        public DownloadLocation SelectedPath {
            get => _selectedPath;
            set { _selectedPath = value;
                OnPropertyChanged("SelectedPath");
            }
        }

        public Configuration Configuartion {
            get => _configuration;
            set { _configuration = value;
                OnPropertyChanged("Configuartion");
            }
        }

    }
}
