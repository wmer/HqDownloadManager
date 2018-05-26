﻿using HqDownloadManager.Helpers;
using HqDownloadManager.WPF.Databases;
using HqDownloadManager.WPF.Models;
using HqDownloadManager.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Tools.MVVM.Commands;
using WPF.Tools.Navigation;

namespace HqDownloadManager.WPF.Commands {
    public class OpenReaderCommand : CommandBase<ChapterReadingProgress> {
        private readonly DownloadHelper _downloadHelper;
        private readonly NavigationManager _navigationManager;
        private readonly ReaderContext _readerContext;

        public OpenReaderCommand(
                        DownloadHelper downloadHelper,
                        NavigationManager navigationManager,
                        ReaderContext readerContext) {
            _downloadHelper = downloadHelper;
            _navigationManager = navigationManager;
            _readerContext = readerContext;
        }

        public override void Execute(ChapterReadingProgress parameter) {
            var pages = _downloadHelper.GetPages(parameter.Location);
            var reading = _readerContext.ChapterReadingProgress
                                        .Find()
                                        .Where(x => x.Location == parameter.Location)
                                        .Execute()
                                        .FirstOrDefault();

            _navigationManager.Navigate<ReaderPage>(parameter.ChapterTitle, pages, reading);
        }
    }
}
