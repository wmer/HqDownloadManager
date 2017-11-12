using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Configuration {
    public class CoreConfiguration {
        public static string BaseDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        private static string _databaseLocation = $"{BaseDirectory}\\Databases";
        private static string _webDriversLocation = $"{BaseDirectory}\\WebDrivers";
        private static string _downloadLocation = $"{BaseDirectory}\\Downloads";

        public static string DownloadLocation {
            get => _downloadLocation;
            set { _downloadLocation = value; }
        }


        public static string WebDriversLocation {
            get => _webDriversLocation;
            set { _webDriversLocation = value; }
        }


        public static string DatabaseLocation {
            get => _databaseLocation;
            set { _databaseLocation = value; }
        }
        
    }
}
