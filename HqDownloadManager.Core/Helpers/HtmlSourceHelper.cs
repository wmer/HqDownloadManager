using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.Helpers {
    public class HtmlSourceHelper {
        private object lockThis = new object();

        public async Task<IDocument> GetSourceCodeFromUrl(String url) {
            var config = new AngleSharp.Configuration().WithDefaultLoader();
            return await BrowsingContext.New(config).OpenAsync(url);
        }

        public IDocument GetSourceCodeFromHtml(String html) {
            lock (lockThis) {
                var parser = new HtmlParser();
                return parser.Parse(html);
            }
        }
    }
}
