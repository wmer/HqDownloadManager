﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DependencyInjectionResolver;
using HqDownloadManager.Controllers;
using HqDownloadManager.Core.Models;
using HqDownloadManager.ViewModels;
using Page = System.Windows.Controls.Page;

namespace HqDownloadManager.Views {

    public partial class HqDetailsPage : Page {
        private readonly DependencyInjection _dependency;
        private HqDetailsController _hqDetailsController;

        private readonly Hq _hq;

        public HqDetailsPage(Hq hq, DependencyInjection dependencyInjection) {
            InitializeComponent();
            _dependency = dependencyInjection;
            _hq = hq;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            _hqDetailsController = _dependency.Resolve<HqDetailsController>();
            if (Resources["Hq"] is HqDetailsViewModel hqResource) {
                hqResource.Hq = _hq;
            }

        }

        private void AddAll_Click(object sender, RoutedEventArgs e) {

        }

        private void AddSelected_Click(object sender, RoutedEventArgs e) {

        }

        private void FollowHq_Click(object sender, RoutedEventArgs e) {

        }

        private void ReadManga_Click(object sender, RoutedEventArgs e) => _hqDetailsController.OpenReader(_hq);
    }
}