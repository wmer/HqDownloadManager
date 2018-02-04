using System;
using System.Collections.Generic;
using System.Text;
using HqDownloadManager.Core.Database;
using HqDownloadManager.Core.Helpers;

namespace HqDownloadManager.Core.Sources {
    public class HqUltimateSource : HqSource {
        public HqUltimateSource(LibraryContext libraryContext, HtmlSourceHelper htmlHelper, BrowserHelper browserHelper) : base(libraryContext, htmlHelper, browserHelper) {
        }
    }
}
