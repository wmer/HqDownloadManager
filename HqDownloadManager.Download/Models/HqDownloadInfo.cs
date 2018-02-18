using HqDownloadManager.Core.Models;
using Repository.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Download.Models {
    public class HqDownloadInfo : DownloadItem {
        public string Path { get; set; }

        public HqDownloadInfo() {

        }

        public HqDownloadInfo(DownloadItem downloadItem) {
            Hq = downloadItem.Hq;
            LastDownloadedChapter = downloadItem.LastDownloadedChapter;
            ActualPage = downloadItem.ActualPage;
            DownloadLocation = downloadItem.DownloadLocation;
            DownloadStarted = downloadItem.DownloadStarted;
            DownloadFinished = downloadItem.DownloadFinished;
        }

        public override bool Equals(object obj) {
            if (obj is HqDownloadInfo model) {
                return Path == model.Path;
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
