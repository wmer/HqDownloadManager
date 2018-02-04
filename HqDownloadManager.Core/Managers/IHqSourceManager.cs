using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Managers {
    public interface IHqSourceManager {
        IHqSourceManager GetInfo<U>(string url, out U model, double timeCache = 3000, bool isFinalized = false, bool whithoutCache = false) where U : ModelBase;
        IHqSourceManager GetUpdates(out List<Update> updates, double timeCache = 189);
        IHqSourceManager GetLibrary(out LibraryPage library, double timeCache = 4320);
        IHqSourceManager GetFinalizedPage(out LibraryPage library, double timeCache = 4320);
        IHqSourceManager GetLetherPage(string lether, out LibraryPage library, double timeCache = 4320);
        IHqSourceManager NextLibraryPage(out LibraryPage library, double timeCache = 4320);
    }
}
