using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core {
    public class SourceManager {
        private SiteHelper siteHelper;
        private Object lockThis = new Object();
        private Object lockThis2 = new Object();
        private Object lockThis3 = new Object();
        private Object lockThis4 = new Object();
        private Object lockThis5 = new Object();
        private Object lockThis6 = new Object();
        private Object lockThis7 = new Object();

        public event ProcessingEventHandler ProcessingProgress;

        public SourceManager(SiteHelper siteHelper) {
            this.siteHelper = siteHelper;
        }

        public ModelBase GetInfo(string url) {
            lock (lockThis) {
                var model = new ModelBase();
                if (siteHelper.IsSupported(url)) {
                    var source = siteHelper.GetHqSourceFromUrl(url);
                    source.ProcessingProgress += Source_ProcessingProgress;
                    if (siteHelper.IsHqPage(url)) {
                        var hq = new Hq();
                        hq = source.GetHqInfo(url);
                        model = hq;
                    }
                    if (siteHelper.IsChapterReader(url)) {
                        var chapter = new Chapter();
                        chapter = source.GetChapterInfo(url);
                        model = chapter;
                    }
                }
                return model;
            }
        }

        public LibraryPage GetLibrary(string url) {
            lock (lockThis2) {
                var library = new LibraryPage();
                if (siteHelper.IsSupported(url)) {
                    var source = siteHelper.GetHqSourceFromUrl(url);
                    source.ProcessingProgress += Source_ProcessingProgress;
                    library = source.GetLibrary(url);
                }

                return library;
            }
        }

        public List<Hq> GetUpdates(string url) {
            lock (lockThis3) {
                var updates = new List<Hq>();
                if (siteHelper.IsSupported(url)) {
                    var source = siteHelper.GetHqSourceFromUrl(url);
                    source.ProcessingProgress += Source_ProcessingProgress;
                    updates = source.GetUpdates(url);
                }
                return updates;
            }
        }

        private void Source_ProcessingProgress(object sender, ProcessingEventArgs ev) {
            lock (lockThis6) {
                OnProcessingProgress(ev);
            }
        }

        protected void OnProcessingProgress(ProcessingEventArgs e) {
            lock (lockThis7) {
                ProcessingProgress?.Invoke(this, e);
            }
        }
    }
}
