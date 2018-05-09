using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HqDownloadManager.WPF.Converters {
    public class WidthToColumnConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is double width && width > 0) {
                return System.Convert.ToInt32(width / 200);
            }
            return 4;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return (double)value * 200;
        }
    }
}
