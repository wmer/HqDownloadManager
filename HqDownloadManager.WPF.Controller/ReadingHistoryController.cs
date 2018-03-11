using HqDownloadManager.Shared.Database;
using HqDownloadManager.Shared.Models;
using HqDownloadManager.Shared.ViewModel.History;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Controller {
    public class ReadingHistoryController : System.Windows.Controls.Page {
        private UserContext _userContext;
        private ReadingHistoryViewModel _readingHisory;

        public ReadingHistoryController(UserContext userContext) {
            _userContext = userContext;
        }

        public virtual void OnLoaded(object sender, RoutedEventArgs e) {
            _readingHisory = ControlsHelper.FindResource<ReadingHistoryViewModel>("MyHistory");
            var listDb = _userContext.ReaderHistory.FindAll();
            var collection = new ObservableCollection<ReadingHistory>();
            foreach (var item in listDb) {
                item.Reader.Hq.Chapters = _userContext.Chapter.Find().Where(x => x.Hq == item.Reader.Hq).Execute();
                item.Reader.ActualChapter = item.Reader.Hq.Chapters[item.Reader.ActualChapterIndex];
                collection.Add(item);
            }
            _readingHisory.Readings = collection;
        }
    }
}
