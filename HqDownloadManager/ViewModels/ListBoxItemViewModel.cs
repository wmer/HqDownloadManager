using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.ViewModels {
    public class ListBoxItemViewModel : ViewModelBase {
        private int _collums;
        private double _width;
        private double _height;

        public int Collums {
            get {
                return _collums;
            }
            set {
                _collums = value;
                OnPropertyChanged("Collums");
            }
        }
        public double Width {
            get {
                return _width;
            }
            set {
                _width = value;
                OnPropertyChanged("Width");
            }
        }
        public double Height {
            get {
                return _height;
            }
            set {
                _height = value;
                OnPropertyChanged("Height");
            }
        }
    }
}
