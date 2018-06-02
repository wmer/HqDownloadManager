using HqDownloadManager.Models;
using HqDownloadManager.WPF.Databases;
using HqDownloadManager.WPF.Models;
using HqDownloadManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HqDownloadManager.WPF.Converters {
    public class DetailsToChapterReadingProgressConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values[1] is DownloadedChapter downloadedChapter) {
                var readingProgress = new ChapterReadingProgress(new ReaderContext()) {
                    ChapterTitle = downloadedChapter.Chapter.Title,
                    HqLocation = (values[0] as DownloadedHq).Location,
                    Location = downloadedChapter.Location
                };

                return readingProgress;
            }

            return values;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
