using HqDownloadManager.Core.Models;
using HqManager.Database;
using HqManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HqManager {
    public class EntryManager {
        private HqManagerContext _context;

        public EntryManager(HqManagerContext context) {
            _context = context;
        }

        public void AddTo(ReadStatus status, Hq hq) =>
        _context.HqEntry.SaveOrReplace(new HqEntry {
            Hq = hq, ReadStatus = status.ToString()
        });

        public void SaveEntry(HqEntry entry) =>
                        _context.HqEntry.SaveOrReplace(entry);

        public string GetReadStatus(Hq hq) {
            if (_context.HqEntry.Find().Where(x => x.Hq == hq).GetOne() is HqEntry entry) {
                return entry.ReadStatus;
            }
            return "Add to...";
        }

        public HqEntry GetHqEntry(Hq hq) =>
            _context.HqEntry.Find().Where(x => x.Hq == hq).GetOne();
    }
}
