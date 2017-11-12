using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;

namespace HqDownloadManager.Core.Sources {
    public abstract class HqSource : IHqSource {
        protected HtmlSourceHelper HtmlHelper;
        protected BrowserHelper BrowserHelper;
        protected bool UsingIe;

        protected object Lock1 = new object();
        protected object Lock2 = new object();
        protected object Lock3 = new object();
        protected object Lock4 = new object();
        protected object Lock5 = new object();
        protected object Lock6 = new object();
        protected object Lock7 = new object();
        protected object Lock8 = new object();
        protected object Lock9 = new object();

        public HqSource(HtmlSourceHelper htmlHelper, BrowserHelper browserHelper) {
            HtmlHelper = htmlHelper;
            BrowserHelper = browserHelper;
        }

        public event ProcessingEventHandler ProcessingProgress;
        public event ProcessingErrorEventHandler ProcessingError;

        public abstract Task<Chapter> GetChapterInfo(string link);

        public abstract Task<Hq> GetHqInfo(string link);

        public abstract Task<LibraryPage> GetLibrary(string linkPage);

        public abstract Task<List<Hq>> GetUpdates(string updatePage);

        protected void OnProcessingProgress(ProcessingEventArgs e) => ProcessingProgress?.Invoke(this, e);

        protected void OnProcessingProgressError(ProcessingErrorEventArgs e) => ProcessingError?.Invoke(this, e);
    }
}
