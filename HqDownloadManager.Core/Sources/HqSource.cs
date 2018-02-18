using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Database;
using HqDownloadManager.Core.Helpers;

namespace HqDownloadManager.Core.Sources {
    public abstract class HqSource : IHqSource {
        protected LibraryContext LibraryContext;
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
        
        protected object LockEvent1 = new object();
        protected object LockEvent2 = new object();

        protected HqSource(LibraryContext libraryContext, HtmlSourceHelper htmlHelper, BrowserHelper browserHelper) {
            LibraryContext = libraryContext;
            HtmlHelper = htmlHelper;
            BrowserHelper = browserHelper;
        }

        public virtual Chapter GetChapterInfo(string link) {
            throw new NotImplementedException();
        }

        public virtual Hq GetHqInfo(string link) {
            throw new NotImplementedException();
        }

        public virtual LibraryPage GetLibrary(string linkPage) {
            throw new NotImplementedException();
        }

        public virtual List<Update> GetUpdates(string url) {
            throw new NotImplementedException();
        }

        protected void OnProcessingProgress(ProcessingEventArgs e) =>
                CoreEventHub.OnProcessingProgress(this, e);

        protected void OnProcessingProgressError(ProcessingErrorEventArgs e) =>
                CoreEventHub.OnProcessingProgressError(this, e);
    }
}
