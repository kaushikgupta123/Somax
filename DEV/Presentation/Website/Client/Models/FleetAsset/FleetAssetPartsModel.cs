using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetAsset
{
    public class FleetAssetPartsModel
    {
        public long EquipmentId { get; set; }
        public string Part_ClientLookupId { get; set; }
        public string Part_Description { get; set; }
        public decimal QuantityNeeded { get; set; }
        public decimal QuantityUsed { get; set; }
        public string Comment { get; set; }
        public long Equipment_Parts_XrefId { get; set; }
        public string PartsSecurity { get; set; }
        public int UpdatedIndex { get; set; }
        public long PartId { get; set; }
    }
}