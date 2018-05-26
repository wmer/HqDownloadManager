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
    public class DetailsToChapterReadingProgressConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is DownloadDetailsViewModel downloadDetails) {
                var readingProgress = new ChapterReadingProgress(new ReaderContext()) {
                    ChapterTitle = downloadDetails.SelectedChapter?.Chapter.Title,
                    HqLocation = downloadDetails.DownloadedHq?.Location,
                    Location = downloadDetails.SelectedChapter?.Location
                };

                return readingProgress;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
