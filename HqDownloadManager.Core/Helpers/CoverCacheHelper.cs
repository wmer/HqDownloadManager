using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Utils;

namespace HqDownloadManager.Core.Helpers {
    internal class CoverCacheHelper {
        private object _lock1 = new object();
        private object _lock2 = new object();
        private string directory;

        public CoverCacheHelper() {
            this.directory = $"{AppDomain.CurrentDomain.BaseDirectory}\\CoverCache";
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
        }

        public void CreateCache(Hq hq) {
            lock (_lock1) {
                if(string.IsNullOrEmpty(hq.CoverSource)) return;
                using (var webClient = new WebClient()) {
                    var pageSource = $"{directory}\\{StringHelper.RemoveSpecialCharacters(hq.Title)}{FormatPage(hq.CoverSource)}";
                    if (!File.Exists(pageSource)) {
                        webClient.DownloadFile(hq.CoverSource, pageSource);
                    }
                    hq.CoverSource = pageSource;
                }
            }
        }

        private string FormatPage(string source) {
            lock (_lock2) {
                var formatPosition = source.LastIndexOf(".");
                var format = source.Substring(formatPosition);
                if (format.Contains("?")) {
                    var posi = format.IndexOf("?");
                    var after = format.Substring(posi);
                    format = format.Replace(after, "");
                }

                if (format.Contains("&")) {
                    var posi = format.IndexOf("&");
                    var after = format.Substring(posi);
                    format = format.Replace(after, "");
                }

                return format;
            }
        }
    }
}
