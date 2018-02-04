using HqDownloadManager.Core.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Helpers {
    public static class CoreEventHub {
        public static event ModelChangeEventHandler ModelChange;
        public static event ModelChangeEventHandler CoverDownload;
        public static event ProcessingEventHandler ProcessingProgress;
        public static event ProcessingErrorEventHandler ProcessingError;

        public static void OnProcessingProgress(object sender, ProcessingEventArgs e) {
            ProcessingProgress?.Invoke(sender, e);
        }

        public static void OnProcessingProgressError(object sender, ProcessingErrorEventArgs e) {
            ProcessingError?.Invoke(sender, e);
        }

        public static void OnModelChange(object sender, ModelChangeEventArgs e) {
            ModelChange?.Invoke(sender, e);
        }
        public static void OnCoverDownload(object sender, ModelChangeEventArgs e) {
            CoverDownload?.Invoke(sender, e);
        }
    }
}
