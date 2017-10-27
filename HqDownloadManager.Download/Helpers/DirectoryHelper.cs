using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HqDownloadManager.Utils;

namespace HqDownloadManager.Download.Helpers {
    internal class DirectoryHelper {
        private object lock1 = new object();
        private object lock2 = new object();
        private object lock3 = new object();
        private object lock5 = new object();
        private object lock6 = new object();

        public string CreateHqDirectory(string rootPath, string hqTitle) {
            lock (lock1) {
                var mainDirectory = FormatMainDirectory(rootPath, hqTitle);
                CreateDirectory(mainDirectory);
                return mainDirectory;
            }
        }

        public string CreateChapterDirectory(string rootPath, string chapterTitle) {
            lock (lock2) {
                if (!Directory.Exists(FormatMainDirectory(rootPath, chapterTitle))) {
                    return CreateHqDirectory(rootPath, chapterTitle);
                }
                throw new Exception($"o capitulo: {chapterTitle} já existe!");
            }
        }

        public void RemoveDirectory(string directory) {
            lock (lock3) {
                var path = directory;
                if (Directory.Exists(path) && directory != "") {
                    Directory.Delete(path, true);
                }
            }
        }

        public string FormatMainDirectory(string rootPath, string subPatch) {
            lock (lock5) {
                String mainDirectory = StringHelper.RemoveSpecialCharacters(subPatch);
                return $"{rootPath}\\{mainDirectory}";
            }
        }

        private bool CreateDirectory(String directory) {
            lock (lock6) {
                var path = directory;
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                    return true;
                }
                return false;
            }
        }
    }
}
