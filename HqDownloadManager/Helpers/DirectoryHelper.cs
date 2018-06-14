using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HqDownloadManager.Helpers {
    internal class DirectoryHelper {
        private object lock1 = new object();
        private object lock2 = new object();
        private object lock3 = new object();
        private object lock5 = new object();
        private object lock6 = new object();

        public string CreateHqDirectory(string rootPath, string hqTitle) {
            lock (lock1) {
                var mainDirectory = FormatMainDirectory(rootPath, hqTitle);
                mainDirectory = CreateDirectory(mainDirectory).Item2;
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

        private (bool, string) CreateDirectory(String directory) {
            lock (lock6) {
                DirectoryInfo dir = null;
                if (!Directory.Exists(directory)) {
                    dir = Directory.CreateDirectory(directory);
                    return (true, dir.FullName);
                }
                dir = new DirectoryInfo(directory);
                return (true, dir.FullName);
            }
        }
    }
}
