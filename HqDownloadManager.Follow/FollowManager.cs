using System;
using System.Collections.Generic;
using System.Text;
using DependencyInjectionResolver;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Follow.CustomEventArgs;
using HqDownloadManager.Follow.Database;
using HqDownloadManager.Follow.Helpers;
using HqDownloadManager.Follow.Models;

namespace HqDownloadManager.Follow {
    public class FollowManager {
        private readonly FollowHelper _followHelper;

        public event FollowEventHandler FollowingHq;

        public FollowManager(DependencyInjection dependencyInjection) : this(AppDomain.CurrentDomain.BaseDirectory, dependencyInjection) {

        }

        public FollowManager(string cachePath, DependencyInjection dependencyInjection) {
            _followHelper = dependencyInjection
                .DefineDependency<FollowContext>(0, cachePath)
                .Resolve<FollowHelper>();
            _followHelper.FollowingHq += FollowHelperOnFollowingHq;
        }

        public void FollowHq(Hq hq) => _followHelper.FollowHq(hq);
        private FollowedHq GetFollowedHq(string link) => _followHelper.GetFollowedHq(link);
        public List<FollowedHq> GetAllFollowedHqs() => _followHelper.GetAllFollowedHqs();

        private void FollowHelperOnFollowingHq(object sender, FollowEventArgs ev) => FollowingHq?.Invoke(this, ev);
    }
}

