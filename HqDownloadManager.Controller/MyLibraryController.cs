using DependencyInjectionResolver;
using HqDownloadManager.Utils;
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

namespace HqDownloadManager.Controller {
    public class MyLibraryController : ListHqControllerBase {
        private HqListViewModel _myDownloads;
        private KeepReadingViewModel _KeepReading;

        public MyLibraryController(DependencyInjection dependencyInjection) : base(dependencyInjection) {
        }

        public override void Init(params object[] values) {
            base.Init(values);
            _myDownloads = ControlsHelper.FindResource<HqListViewModel>("HqList");
            _KeepReading = ControlsHelper.FindResource<KeepReadingViewModel>("KeepReading");

        }

        public void KeepReading<T>(object clickedItem) {
            var selected = clickedItem as ReaderViewModel;
            NavigationHelper.Navigate<T>($"{selected.Hq.Title}", selected.Hq);
        }

        public async Task ShowReadings() {
            var userReadings = UserLibrary.UserReadings.FindAll();
            var list = new List<ReaderViewModel>();
            foreach (var userReading in userReadings) {
                var reader = userReading.HqReaderViewModel.ToObject<ReaderViewModel>();
                _KeepReading.Date = userReading.Date;
                list.Add(reader);
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

        public async Task DeleteHq(Hq hq, bool eraseFiles = false) {
            await DownloadManager.DeleteDownloadInfo(hq.Link, eraseFiles);
        }

        public void ClearLists() {
            _myDownloads.Hqs.Clear();
            _KeepReading.Readings.Clear();
        }
    }
}
