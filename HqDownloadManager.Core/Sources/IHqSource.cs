using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Sources {
    public interface IHqSource {
        Hq GetHqInfo(String link);
        Chapter GetChapterInfo(String link);
        LibraryPage GetLibrary(String linkPage);
        List<Hq> GetUpdates(String updatePage);

        event ProcessingEventHandler ProcessingProgress; 
    }
}
