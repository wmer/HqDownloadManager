using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Core.Configuration {
    public class CoreConfiguration {
        public static string BaseDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        private static string _databaseLocation = $"{BaseDirectory}\\Databases";
        private static string _webDriversLocation = $"{BaseDirectory}\\WebDrivers";
        private static string _cacheLocation = $"{BaseDirectory}\\Cache";
        private static string _coverLocation = $"{CacheLocation}\\Covers";

        public static string CacheLocation {
            get { return _cacheLocation; }
            set { _cacheLocation =  value; }
        }

        public static string WebDriversLocation {
            get => _webDriversLocation;
            set { _webDriversLocation = value; }
        }

        public static string DatabaseLocation {
            get => _databaseLocation;
            set { _databaseLocation = value; }
        }

        public static string CoverLocaion {
            get => _coverLocation;
            set { _coverLocation = value; }
        }

    }
}
