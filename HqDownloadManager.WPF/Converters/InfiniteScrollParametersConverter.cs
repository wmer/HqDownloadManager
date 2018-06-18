using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace HqDownloadManager.WPF.Converters {
    public class InfiniteScrollParametersConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var dic = new Dictionary<string, object> {
                ["LibraryViewModel"] = values[0],
            };
            if (values[1] is ScrollChangedEventArgs eventArgs) {
                var scrollViwer = eventArgs.OriginalSource as ScrollViewer;
                dic["VerticalOffset"] = scrollViwer.VerticalOffset;
                dic["ScrollableHeight"] = scrollViwer.ScrollableHeight;
            }
            return dic;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            if (value is Dictionary<string, object> dic) {
                var objects = new object[dic.Count];
                var i = 0;
                foreach (var parKey in dic) {
                    objects[i] = parKey.Value;
                    i++;
                }

                return objects;
            }
            return null;
        }
    }
}
