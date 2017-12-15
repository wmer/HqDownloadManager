using DependencyInjectionResolver;
using Utils;
using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Controller.ViewModel.Shared;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Controller.ViewModel.MyLibrary;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Core;

namespace HqDownloadManager.Controller {
    public class MyLibraryController : ListHqControllerBase {
        private HqListViewModel _myDownloads;
        private KeepReadingViewModel _KeepReading;

        public MyLibraryController() : base() {
        }

        public override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            _myDownloads = ControlsHelper.FindResource<HqListViewModel>("HqList");
            _KeepReading = ControlsHelper.FindResource<KeepReadingViewModel>("KeepReading");

        }

        public void KeepReading<T>(object clickedItem) {
            var selected = clickedItem as ReaderViewModel;
            NavigationHelper.Navigate<T>($"{selected.Hq.Title}", selected);
        }

        public async Task ShowReadings() {
            var userReadings = UserLibrary.UserReadings.FindAll();
            var list = new List<ReaderViewModel>();
            foreach (var userReading in userReadings) {
                _KeepReading.Date = userReading.Date;
                list.Add(userReading.Reading.ToObject<ReaderViewModel>());
            }
            list = list.Reverse<ReaderViewModel>().ToList();
            _KeepReading.Readings = new ObservableCollection<ReaderViewModel>(list);
        }

        public async Task ShowDownloads() {
            var downloads = await DownloadManager.GetDownloadedHqsInfo();
            var _hqList = new ObservableCollection<Hq>();
            foreach (var hqDownloaded in downloads) {
                var hq = hqDownloaded.HqDownloaded.ToObject<Hq>();
                _hqList.Add(hq);
            }

            var listOrde = _hqList.OrderBy(x => x.Title).ToList();
            _myDownloads.Hqs = new ObservableCollection<Hq>(listOrde);
        }

        public override async Task OpenHqDetails<T>(bool isFinalized = false) {
            var Selectedhq = await GetSelectedHq(isFinalized);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                NavigationHelper.Navigate<T>("Detalhes", Selectedhq);
            });
        }

        public async Task DeleteHq(Hq hq, bool eraseFiles = false) {
            await DownloadManager.DeleteDownloadInfo(hq.Link, eraseFiles);
        }

        public void ClearLists() {
            _myDownloads.Hqs.Clear();
            _KeepReading.Readings.Clear();
        }
    }
}
