using HqDownloadManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace HqDownloadManager.WPF.Converters {
    public class PageChangedParametersConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var tuple = new Tuple<ScrollChangedEventArgs, ListView, ReaderViewModel>(
                    values[0] as ScrollChangedEventArgs,
                    values[1] as ListView,
                    values[2] as ReaderViewModel
                );

            return tuple;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
