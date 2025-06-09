using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class VendorAssetMgtWidgetModal
    {
        public long VendorId { get; set; }
        public long VendorAssetMgtId { get; set; }
        public string Company { get; set; }
        public string Contact { get; set; }
        public string Contract { get; set; }
        public bool AssetMgtRequired { get; set; }
        public DateTime? AssetMgtExpireDate { get; set; }
        public bool AssetMgtOverride { get; set; }
        public DateTime? AssetMgtOverrideDate { get; set; }
    }
}