using HqDownloadManager.Controller.Models;
using HqDownloadManager.Controller.ViewModel.Follows;
using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using Utils;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.Collections.ObjectModel;

namespace HqDownloadManager.Controller {
    public class FollowController : ControllerBase {
        private FollowUpdatesViewModel _followUpdates;

        public override void OnLoaded(object sender, RoutedEventArgs e) {
            base.OnLoaded(sender, e);
            _followUpdates = ControlsHelper.FindResource<FollowUpdatesViewModel>("FollowList");
        }

        public void ShowFollows() {
            var follows = FollowManager.GetAllFollowedHqs();
            var list = new List<FollowItem>();
            foreach (var follow in follows) {
                list.Add(new FollowItem {
                    Hq = follow.Hq.ToObject<Hq>(),
                    Date = follow.Time,
                    Updates = new ObservableCollection<Chapter>(UpdateManager.GetUpdatesFrom(follow.Link))
                });
            }
            _followUpdates.Updates = new ObservableCollection<FollowItem>(list);
        }
    }
}
