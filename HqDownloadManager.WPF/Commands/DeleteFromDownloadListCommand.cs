using HqDownloadManager.Download;
using HqDownloadManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;

namespace HqDownloadManager.WPF.Commands {
    public class DeleteFromDownloadListCommand : CommandBase<DownloadItem> {
        private DownloadManager _downloadManager;

        public DeleteFromDownloadListCommand(DownloadManager downloadManager) {
            _downloadManager = downloadManager;
        }

        public override void Execute(DownloadItem parameter) =>
                    _downloadManager.ExcludeFromDownloadList(parameter);
    }
}
