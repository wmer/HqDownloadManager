using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Resources {
    public class ListBoxItemTemplateSizes : INotifyPropertyChanged {
        private int collums;
        private double width;
        private double height;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Collums {
            get {
                return collums;
            }
            set {
                collums = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Collums"));
            }
        }
        public double Width {
            get {
                return width;
            }
            set {
                width = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Width"));
            }
        }
        public double Height {
            get {
                return height;
            }
            set {
                height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs ev) {
            PropertyChanged?.Invoke(this, ev);
        }
    }
}
