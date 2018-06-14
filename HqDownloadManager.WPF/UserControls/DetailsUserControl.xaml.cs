using HqDownloadManager.WPF.UserControls.ViewModels;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.UserControls {
    /// <summary>
    /// Interação lógica para UpdateDetailsUserControl.xam
    /// </summary>
    public partial class DetailsUserControl : UserControl {
        private DetailsViewModel _detailsViewModel;

        public DetailsUserControl() {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            _detailsViewModel = Resources["DetailsView"] as DetailsViewModel;
            if (ShowUpdateSection) {
                HqChaptersLabel.Margin = new Thickness(5, 445, 0, 0);
                HqChapters.Margin = new Thickness(5, 475, 0, 0);
            } else {
                HqChaptersLabel.Margin = new Thickness(5, 295, 0, 0);
                HqChapters.Margin = new Thickness(5, 330, 0, 0);
            }
        }

        public static readonly DependencyProperty OpenedProperty =
           DependencyProperty.Register("Opened", typeof(bool), typeof(DetailsUserControl), null);

        public static readonly DependencyProperty ShowUpdateSectionProperty =
           DependencyProperty.Register("ShowUpdateSection", typeof(bool), typeof(DetailsUserControl), null);

        public static readonly DependencyProperty HqProperty =
                DependencyProperty.Register("Hq", typeof(Hq), typeof(DetailsUserControl), null);

        public static readonly DependencyProperty UpdateProperty =
                DependencyProperty.Register("Update", typeof(Update), typeof(DetailsUserControl), null);

        public bool Opened {
            get => (bool)GetValue(OpenedProperty);
            set => SetValue(OpenedProperty, value);
        }

        public bool ShowUpdateSection {
            get => (bool)GetValue(ShowUpdateSectionProperty);
            set => SetValue(ShowUpdateSectionProperty, value);
        }

        public Hq Hq {
            get => (Hq)GetValue(HqProperty);
            set {
                SetValue(HqProperty, value);
                _detailsViewModel.AddToDownload.RaiseCanExecuteChanged();
            }
        }

        public Update Update {
            get => (Update)GetValue(UpdateProperty);
            set {
                SetValue(UpdateProperty, value);
                _detailsViewModel.DownloadUpdates.RaiseCanExecuteChanged();
            }
        }

        public Chapter SelectedChapter { get; set; }

        private List<Chapter> _selectedchapter;

        public List<Chapter> SelectedChapters {
            get { return _selectedchapter; }
            set {
                _selectedchapter = value;
                _detailsViewModel.DownloadSelected.RaiseCanExecuteChanged();
            }
        }

        private void HqChapters_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (HqChapters.SelectedItems != null && HqChapters.SelectedItems.Count > 0) {
                var selected = new List<Chapter>();
                foreach (var item in HqChapters.SelectedItems) {
                    selected.Add(item as Chapter);
                }

                SelectedChapters = selected;
            }
        }
    }
}
