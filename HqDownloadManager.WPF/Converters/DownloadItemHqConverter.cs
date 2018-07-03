using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using HqDownloadManager.Helpers;

namespace HqDownloadManager.WPF.Converters {
    public class DownloadItemHqConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var hqBytes = value as byte[];
            return hqBytes.ToObject<Hq>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value.ToBytes();
        }
    }
}
