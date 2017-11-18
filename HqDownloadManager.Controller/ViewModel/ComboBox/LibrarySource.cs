using HqDownloadManager.Controller.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Controller.ViewModel.ComboBox {
    public class LibrarySource : ComboBoxViewModelBase {
        public LibrarySource() {
            SourceLinks = new ObservableCollection<SourceLink>
            {
                new SourceLink {SourceName = "MangaHost", Link = "https://mangashost.com/mangas"},
                new SourceLink {SourceName = "YesMangas", Link = "https://ymangas.com/mangas"},
                new SourceLink {SourceName = "UnionMangas", Link = "http://unionmangas.net/mangas"},
                new SourceLink
                {
                    SourceName = "MangasProject",
                    Link = "https://mangas.zlx.com.br/lista-de-mangas/ordenar-por-nome/todos"
                }
            };

            SelectedSourceLink = SourceLinks[0];
        }
    }
}
