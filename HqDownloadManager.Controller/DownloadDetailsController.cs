using HqDownloadManager.Controller.Models;
using HqDownloadManager.Controller.ViewModel.MyLibrary;
using HqDownloadManager.Controller.ViewModel.Reader;
using HqDownloadManager.Core.Models;
using Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller {
    public class DownloadDetailsController : ControllerBase {
        public DownloadDetailsController() : base() {
        }

        public void LoadDetails(Hq hq) {
            var downloadViewModel = ControlsHelper.FindResource<DownloadDetailsViewModel>("downloadViewModel");
            downloadViewModel.Hq = hq;
            var i = 1;
            var readerViews = new ObservableCollection<ReaderViewModel>();
            foreach (var chap in hq.Chapters) {
                var reader = new ReaderViewModel {
                    Hq = hq
                };
                if (UserLibrary.UserReadings.FindOne(chap.Link) is UserReading readings) {
                    reader = readings.Reading.ToObject<ReaderViewModel>();
                } else {
                    reader.ActualChapter = chap;
                    reader.ActualChapterIndex = i - 1;
                    if (i > 1) {
                        reader.PreviousChapter = hq.Chapters[i - 2];
                    }
                    if (i < hq.Chapters.Count) {
                        reader.NextChapter = hq.Chapters[i];
                    }
                }
                readerViews.Add(reader);
                i++;
            }
            downloadViewModel.ReaderViewModel = readerViews;
            downloadViewModel.Updates = UpdateManager.GetUpdatesFrom(hq.Link);
        }

        public void Read<T>(ReaderViewModel reader) =>  NavigationHelper.Navigate<T>($"{reader.Hq.Title}", reader);

        public async Task DeleteHq<T>(Hq hq, bool eraseFolders = false) {
            await DownloadManager.DeleteDownloadInfo(hq.Link, eraseFolders);
            NavigationHelper.Navigate<T>("Meus Downloads");
        }
    }
}
