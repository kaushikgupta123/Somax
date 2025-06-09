using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class RepariableSpareAssetLookupModel
    {
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Make { get; set; }
        public long TotalCount { get; set; }
        public long? EquipmentId { get; set; }

    }
}