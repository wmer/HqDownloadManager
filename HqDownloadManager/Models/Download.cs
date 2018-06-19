using ADO.ORM.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HqDownloadManager.Models {
    public class Download : IEquatable<Download> {
        [PrimaryKey]
        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as Download);
        }

        public bool Equals(Download other) {
            return other != null &&
                   Location == other.Location;
        }

        public override int GetHashCode() {
            return 1369928374 + EqualityComparer<string>.Default.GetHashCode(Location);
        }
    }
}
