using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HqDownloadManager.WPF.Helpers {
    public class DownloadHelper {
        [STAThread]
        public string SelectFolder() {
            var selectedPath = "";
            var selectFolderDialog = new FolderBrowserDialog();
            if (selectFolderDialog.ShowDialog() == DialogResult.OK) {
                selectedPath = selectFolderDialog.SelectedPath;
            }
            return selectedPath;
        }
    }
}
