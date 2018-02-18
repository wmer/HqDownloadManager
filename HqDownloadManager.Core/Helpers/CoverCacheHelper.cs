using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HqDownloadManager.Core.Configuration;
using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Models;
using Utils;

namespace HqDownloadManager.Core.Helpers {
    public class CoverCacheHelper {
        private object _lock1 = new object();
        private object _lock2 = new object();
        private string directory;

        public CoverCacheHelper() {
            this.directory = CoreConfiguration.CacheLocation;
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
        }

        public string CreateCache(Hq hq) {
            lock (_lock1) {
                if (string.IsNullOrEmpty((string)hq.CoverSource)) return hq.CoverSource;
                using (var webClient = new WebClient()) {
                    try {
                        var pageSource = $"{directory}\\{StringHelper.RemoveSpecialCharacters((string)hq.Title)}{FormatPage((string)hq.CoverSource)}";
                        if (!File.Exists(pageSource)) {
                            webClient.DownloadFile((string)hq.CoverSource, pageSource);
                        }
                        return pageSource;
                    } catch {
                        return hq.CoverSource;
                    }
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
