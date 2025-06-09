using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class ChangeFleetAssetIDModel
    {
        public long EquipmentId { get; set; }
        public string ClientLookupId { get; set; }
        public int UpdateIndex { get; set; }
    }
}