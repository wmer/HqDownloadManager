using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Core.Models;
using HqDownloadManager.ViewModels.Reader;
using Windows.UI.Xaml.Data;

namespace HqDownloadManager.Controllers {
    public class HqReaderController : Controller {
        private ReaderViewModel _readerViewModel;
        private ICollectionView _collectionView;

        public HqReaderController(DependencyInjection dependencyInjection) : base(dependencyInjection) {
        }

        public void InitReader(Hq hq) {
            _readerViewModel = ControlsHelper.FindResource<ReaderViewModel>("ReaderControl");
            _collectionView = ControlsHelper.FindResource<CollectionViewSource>("Reader")?.View;
            _readerViewModel.ActualChapter = SourceManager.GetInfo(hq.Chapters[0].Link) as Chapter;
        }
    }
}
