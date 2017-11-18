using DependencyInjectionResolver;
using HqDownloadManager.Core.Models;
using HqDownloadManager.FollowUpdate.CustomEventArgs;
using HqDownloadManager.FollowUpdate.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.FollowUpdate
{
    public class UpdateManager {
        private readonly UpdateHelper _updateHelper;

        public event UpdateEventHandler UpdateStart;
        public event UpdateEventHandler UpdateEnd;

        public UpdateManager(DependencyInjection dependencyInjection) {
            _updateHelper = dependencyInjection
                .Resolve<UpdateHelper>();
            _updateHelper.UpdateStart += UpdateHelperOnUpdateStart;
            _updateHelper.UpdateEnd += UpdateHelperOnUpdateEnd;
        }

        public List<Chapter> GetUpdatesFrom(string link) => _updateHelper.GetUpdates(link);

        private void UpdateHelperOnUpdateStart(object sender, UpdateEventArgs ev) => UpdateStart?.Invoke(this, ev);

        private void UpdateHelperOnUpdateEnd(object sender, UpdateEventArgs ev) => UpdateEnd?.Invoke(this, ev);
    }
}
