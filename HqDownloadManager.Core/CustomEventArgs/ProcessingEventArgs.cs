using HqDownloadManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqDownloadManager.Core.CustomEventArgs {
    public class ProcessingEventArgs : EventArgs {
        public DateTime Time { get; private set; }
        public String StateMessage { get; private set; }
        public ModelBase Item { get; set; }

        public ProcessingEventArgs(DateTime time, String stateMessage) {
            Time = time;
            StateMessage = stateMessage;
        }

        public ProcessingEventArgs(DateTime time, ModelBase item, String stateMessage) {
            Time = time;
            StateMessage = stateMessage;
            Item = item;
        }
    }

    public delegate void ProcessingEventHandler(object sender, ProcessingEventArgs ev);
}
