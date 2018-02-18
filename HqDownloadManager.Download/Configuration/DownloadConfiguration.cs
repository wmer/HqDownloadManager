using HqDownloadManager.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Download.Configuration {
    public class DownloadConfiguration : CoreConfiguration {
        private static List<string> _downloadLocation = new List<string> {
            $"{BaseDirectory}\\Downloads"
        };

        public static List<string> DownloadLocations {
            get => _downloadLocation;
            set { _downloadLocation = value; }
        }
    }
}
