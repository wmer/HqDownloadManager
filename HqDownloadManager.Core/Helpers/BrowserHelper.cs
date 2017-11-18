using HqDownloadManager.Core.Configuration;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Helpers {
    internal class BrowserHelper {

        private Object lockThis = new Object();

        public RemoteWebDriver GetDriver(string url) {
            var driver = new PhantomJSDriver(CoreConfiguration.WebDriversLocation) {
                Url = url
            };
            driver.Navigate();
            return driver;
        }

        public RemoteWebDriver GetPhantomMobile(string url) {
            var options = new PhantomJSOptions();
            options.AddAdditionalCapability("phantomjs.page.settings.userAgent",
           @"Mozilla/5.0 (Android 4.4; Mobile; rv:41.0) Gecko/41.0 Firefox/41.0");

            var driver = new PhantomJSDriver(CoreConfiguration.WebDriversLocation, options) {
                Url = url
            };
            driver.Navigate();
            return driver;
        }
    }
}
