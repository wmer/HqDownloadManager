using HqDownloadManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HqDownloadManager.Shared.ViewModel.MainWindow {
    public class NavigationViewModel : ViewModelBase {
        private ObservableCollection<MenuButton> _buttons;
        private MenuButton _selectedButton;
        private string _pageTitle;
        private bool _opened;
        private bool _menuVisibility;

        public NavigationViewModel() {
            Buttons = new ObservableCollection<MenuButton> {
                new MenuButton{ Icon = "\uf0c9", Label = "Menu"},
                new MenuButton{ Icon = "\uf017", Label = "Atualizações"},
                new MenuButton{ Icon = "\uf02d", Label = "Biblioteca"},
                new MenuButton{ Icon = "\uf019", Label = "Meus Downloads"},
                new MenuButton{ Icon = "\uf03a", Label = "Minha Lista de Mangás"},
                new MenuButton{ Icon = "\uf309", Label = "Gerenciador de Downloads"},
                new MenuButton{ Icon = "\uf085", Label = "Configurações"},
            };
        }

        public bool Opened {
            get => _opened;
            set { _opened = value;
                OnPropertyChanged("Opened");
            }
        }

        public bool MenuVisibility {
            get => _menuVisibility;
            set {
                _menuVisibility = value;
                OnPropertyChanged("MenuVisibility");
            }
        }

        public string PageTitle {
            get => _pageTitle;
            set { _pageTitle = value;
                OnPropertyChanged("PageTitle");
            }
        }

        public MenuButton SelectedButton {
            get => _selectedButton;
            set { _selectedButton = value;
                OnPropertyChanged("SelectedButton");
            }
        }

        public ObservableCollection<MenuButton> Buttons {
            get => _buttons;
            set { _buttons = value;
                OnPropertyChanged("Buttons");
            }
        }

    }
}
