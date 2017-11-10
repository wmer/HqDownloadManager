using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Models;

namespace HqDownloadManager.ViewModels.ComboBox {
    public class UpdateSource : ComboBoxViewModelBase {
        public UpdateSource() {
            SourceLinks = new ObservableCollection<SourceLink>
            {
                new SourceLink {SourceName = "MangaHost", Link = "https://mangashost.com/"},
                new SourceLink {SourceName = "YesMangas", Link = "https://ymangas.com/"},
               // new SourceLink { SourceName = "UnionMangas", Link = "http://unionmangas.net" },
                new SourceLink {SourceName = "MangasProject", Link = "https://mangas.zlx.com.br"}
            };

            SelectedSourceLink = SourceLinks[0];
        }
    }
}
