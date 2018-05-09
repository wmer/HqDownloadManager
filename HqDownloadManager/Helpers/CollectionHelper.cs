using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Helpers {
    public static class CollectionHelper {
        public static void AddRange<T, U>(this Dictionary<T, U> target, Dictionary<T, U> source) {
            if (source != null && source.Count > 0) {
                foreach (var item in source) {
                    if (!target.ContainsKey(item.Key)) {
                        target.Add(item.Key, item.Value);
                    }
                }
            }
        }
    }
}
