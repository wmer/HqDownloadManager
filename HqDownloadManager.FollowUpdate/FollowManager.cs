using DependencyInjectionResolver;
using HqDownloadManager.Core.Models;
using HqDownloadManager.FollowUpdate.CustomEventArgs;
using HqDownloadManager.FollowUpdate.Helpers;
using HqDownloadManager.FollowUpdate.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.FollowUpdate
{
    public class FollowManager {
        private readonly FollowHelper _followHelper;
        
        public FollowManager(DependencyInjection dependencyInjection) {
            _followHelper = dependencyInjection
                .Resolve<FollowHelper>();
        }

        public void FollowHq(Hq hq) => _followHelper.FollowHq(hq);
        public Hq GetFollowedHq(string link) => _followHelper.GetFollowedHq(link);
        public List<Hq> GetAllFollowedHqs() => _followHelper.GetAllFollowedHqs();
    }
}
