using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Extra {
    public class ItemsDataTemplateSelector : DataTemplateSelector {
        public DataTemplate FirstItem { get; set; }
        public DataTemplate OtherItem { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            var lv = container.Find<ListView>().FirstOrDefault();
            if (lv != null) {
                if (lv.Items.Count == 1) {
                    return FirstItem;
                }

                int i = lv.Items.IndexOf(item);
                if (i == 0) {
                    return FirstItem;
                } else {
                    return OtherItem;
                }
            }
            return FirstItem;
        }
    }
}
