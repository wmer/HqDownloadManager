using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Helpers {
    internal class HtmlSourceHelper {
        private Object lockThis = new Object();
        private Object lockThis2 = new Object();

        public IDocument GetSourceCodeFromUrl(String url) {
            lock (lockThis) {
                var config = new Configuration().WithDefaultLoader();
                var source = BrowsingContext.New(config).OpenAsync(url).Result;
                return source;
            }
        }

        public IDocument GetSourceCodeFromHtml(String html) {
            lock (lockThis2) {
                var parser = new HtmlParser();
                return parser.Parse(html);
            }
        }
    }
}
