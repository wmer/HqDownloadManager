using HqDownloadManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.Xaml;

namespace HqDownloadManager.WPF.Commands {
    public class ChangePageCommand : CommandBase<Tuple<ScrollChangedEventArgs, ListView, ReaderViewModel>> {
        private int _actualPage;

        public override void Execute(Tuple<ScrollChangedEventArgs, ListView, ReaderViewModel> parameter) {
            var listViewIntem = parameter.Item2.Find<ListViewItem>().FirstOrDefault();
            if (listViewIntem != null) {
                var scroll = parameter.Item1.OriginalSource as ScrollViewer;
                var posi = scroll.HorizontalOffset;
                var width = listViewIntem.ActualWidth;
                var index = Convert.ToInt32(posi / width);
                if (index != _actualPage) {
                    _actualPage = index;
                    parameter.Item3.ActualPageIndex = index;
                    if (parameter.Item3.ActualChapter != null &&
                        parameter.Item3.ActualChapter.Chapter.Pages != null &&
                        parameter.Item3.ActualChapter.Chapter.Pages.Count() > 0) {
                        parameter.Item2.ScrollIntoView(parameter.Item3.ActualChapter.Chapter.Pages[index]);
                    }
                }
            }
        }
    }
}
