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


        public UpdateManager(DependencyInjection dependencyInjection) {
            _updateHelper = dependencyInjection
                .Resolve<UpdateHelper>();
        }

        public List<Chapter> GetUpdatesFrom(string link) => _updateHelper.GetUpdates(link);
    }
}
