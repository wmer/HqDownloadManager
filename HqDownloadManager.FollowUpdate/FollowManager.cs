﻿using DependencyInjectionResolver;
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

        public event FollowEventHandler FollowingHq;

        public FollowManager(DependencyInjection dependencyInjection) {
            _followHelper = dependencyInjection
                .Resolve<FollowHelper>();
            _followHelper.FollowingHq += FollowHelperOnFollowingHq;
        }

        public void FollowHq(Hq hq) => _followHelper.FollowHq(hq);
        public FollowedHq GetFollowedHq(string link) => _followHelper.GetFollowedHq(link);
        public List<FollowedHq> GetAllFollowedHqs() => _followHelper.GetAllFollowedHqs();

        private void FollowHelperOnFollowingHq(object sender, FollowEventArgs ev) => FollowingHq?.Invoke(this, ev);
    }
}
