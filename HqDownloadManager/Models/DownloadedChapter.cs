﻿using ADO.ORM.Attributes;
using MangaScraping.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Models {
    public class DownloadedChapter : Download {
        [Ignore]
        public Chapter Chapter { get; set; }
    }
}
