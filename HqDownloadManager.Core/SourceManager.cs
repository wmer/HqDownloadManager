using HqDownloadManager.Core.CustomEventArgs;
using HqDownloadManager.Core.Helpers;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectionResolver;
using HqDownloadManager.Core.Database;
using Newtonsoft.Json;
using Utils;
using HqDownloadManager.Core.Managers;
using HqDownloadManager.Core.Sources;

namespace HqDownloadManager.Core {
    public class SourceManager {
        private SiteHelper _siteHelper;
        private MangaHostSourceManager _mangaHostManager;
        private UnionMangasSourceManager _unionManager;
        private YesMangasSourceManager _yesManager;
        private MangasProjectSourceManager _projectManager;
        private HipercoolSourceManager _hipercoolManager;
        private HqUltimateSourceManager _hqUltimateManager;
        private Dictionary<SourcesEnum, IHqSourceManager> _sources;

        private readonly Object _lockThis = new Object();

        public SourceManager(MangaHostSourceManager mangaHpstManager,
                    UnionMangasSourceManager unionManager,
                    YesMangasSourceManager yesManager,
                    MangasProjectSourceManager projectManager,
                    HipercoolSourceManager hipercoolManager,
                    HqUltimateSourceManager hqUltimateManager, 
                    SiteHelper siteHelper) {
            _mangaHostManager = mangaHpstManager;
            _unionManager = unionManager;
            _yesManager = yesManager;
            _projectManager = projectManager;
            _hipercoolManager = hipercoolManager;
            _hqUltimateManager = hqUltimateManager;
            _siteHelper = siteHelper;
            _sources = new Dictionary<SourcesEnum, IHqSourceManager> {
                [SourcesEnum.MangaHost] = _mangaHostManager,
                [SourcesEnum.UnionMangas] = _unionManager,
                [SourcesEnum.YesMangas] = _yesManager,
                [SourcesEnum.MangasProject] = _projectManager,
                [SourcesEnum.Hipercool] = _hipercoolManager,
                [SourcesEnum.HqUltimate] = _hqUltimateManager
            };
        }

        public Dictionary<string, IHqSourceManager> GetSources() {
            lock (_lockThis) {

                var sourceNanagers = new Dictionary<string, IHqSourceManager> {
                    ["MangaHost"] = _mangaHostManager,
                    ["UnionMangas"] = _unionManager,
                    ["YesMangas"] = _yesManager,
                    ["MangasProject"] = _projectManager,
                    ["Hipercool"] = _hipercoolManager,
                    ["HqUltimate"] = _hqUltimateManager
                };

                return sourceNanagers;
            }
        }

        public IHqSourceManager GetSourceFromLink(string link) => _siteHelper.GetHqSourceFromUrl(link);
        public IHqSourceManager GetSpurce(SourcesEnum source) => _sources[source];
    }
}
