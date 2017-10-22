using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Core.Database;

namespace HqDownloadManager.Core.Sources {
    internal abstract class HqSource : IHqSource {
        public LibraryContext _libraryContext;

        public HqSource(LibraryContext libraryContext) {
            _libraryContext = libraryContext;
        }

        public event ProcessingEventHandler ProcessingProgress;
        protected object lockEvent1 = new object();
        protected bool lockedEvent;

        public virtual Chapter GetChapterInfo(string link) {
            throw new NotImplementedException();
        }

        public virtual Hq GetHqInfo(string link) {
            throw new NotImplementedException();
        }

        public virtual LibraryPage GetLibrary(string linkPage) {
            throw new NotImplementedException();
        }

        public virtual List<Hq> GetUpdates(string updatePage) {
            throw new NotImplementedException();
        }

        protected void OnProcessingProgress(ProcessingEventArgs e) {
            lock (lockEvent1) {
                if (!lockedEvent) {
                    lockedEvent = true;
                    ProcessingProgress?.Invoke(this, e);
                    lockedEvent = false;
                }
            }
        }
    }
}
