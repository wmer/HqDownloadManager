using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Sources
{
    public interface IHqSource {
        Task<Hq> GetHqInfo(String link);
        Task<Chapter> GetChapterInfo(String link);
        Task<LibraryPage> GetLibrary(String linkPage);
        Task<List<Hq>> GetUpdates(String updatePage);

        event ProcessingEventHandler ProcessingProgress;
        event ProcessingErrorEventHandler ProcessingError;
    }
}
