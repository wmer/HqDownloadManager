using System;
using System.Collections.Generic;
using System.Text;
using DependencyInjectionResolver;
using HqDownloadManager.Core.Models;
using HqDownloadManager.Follow.CustomEventArgs;
using HqDownloadManager.Follow.Database;
using HqDownloadManager.Follow.Helper;

namespace HqDownloadManager.Follow {
    public class UpdateManager {
        private readonly UpdateHelper _updateHelper;

        public event UpdateEventHandler UpdateStart;
        public event UpdateEventHandler UpdateEnd;

        public UpdateManager(DependencyInjection dependencyInjection) : this(AppDomain.CurrentDomain.BaseDirectory, dependencyInjection)
        {
            
        }

        public UpdateManager(string cachePath, DependencyInjection dependencyInjection) {
            _updateHelper = dependencyInjection
                .DefineDependency<FollowContext>(0, cachePath)
                .Resolve<UpdateHelper>();
            _updateHelper.UpdateStart += UpdateHelperOnUpdateStart;
            _updateHelper.UpdateEnd += UpdateHelperOnUpdateEnd;
        }

        public List<Chapter> GetUpdatesFrom(string link) => _updateHelper.GetUpdates(link);

        private void UpdateHelperOnUpdateStart(object sender, UpdateEventArgs ev) => UpdateStart?.Invoke(this, ev);

        private void UpdateHelperOnUpdateEnd(object sender, UpdateEventArgs ev) => UpdateEnd?.Invoke(this, ev);
    }
}
