using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.ViewModels {
    public class PaneViewModel : ViewModelBase {
        private int _width;

        public int Width {
            get { return _width; }
            set { _width = value;
                OnPropertyChanged("Width");
            }
        }

    }
}
