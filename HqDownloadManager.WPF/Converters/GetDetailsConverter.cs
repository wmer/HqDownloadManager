using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HqDownloadManager.WPF.Converters {
    public class GetDetailsConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var dic = new Dictionary<string, object> {
                ["SelectedSource"] = values[0],
                ["SelectedUpdate"] = values[1],
                ["DetailsViewModel"] = values[2]
            };
            return dic;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            if (value is Dictionary<string, object> dic) {
                return new object[] { dic["SelectedSource"], dic["SelectedUpdate"], dic["DetailsViewModel"] };
            }
            return new object[3];
        }
    }
}
