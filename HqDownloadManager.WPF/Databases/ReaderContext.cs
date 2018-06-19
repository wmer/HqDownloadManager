using ADO.ORM;
using ADO.ORM.Attributes;
using ADO.ORM.Core.SqLite;
using HqDownloadManager.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.WPF.Databases {
    public class ReaderContext : SqLiteContext {
        [Ignore]
        public static string DatabaseLocation { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}\\Databases";
        public ReaderContext() : base(DatabaseLocation, "ReaderContext") { }

        public Repository<ChapterReadingProgress> ChapterReadingProgress { get; set; }
    }
}
