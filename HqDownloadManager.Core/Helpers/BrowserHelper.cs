using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Helpers {
    internal class BrowserHelper {
        private string _webDriverPath;

        private Object lockThis = new Object();

        public BrowserHelper(string webDriverPath) {
            _webDriverPath = webDriverPath;
        }

        public RemoteWebDriver GetDriver(string url) {
            var driver = new PhantomJSDriver(_webDriverPath) {
                Url = url
            };
            driver.Navigate();
            return driver;
        }
    }
}
