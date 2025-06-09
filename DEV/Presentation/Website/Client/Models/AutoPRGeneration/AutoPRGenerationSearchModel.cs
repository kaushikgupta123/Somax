using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class AutoPRGenerationSearchModel
    {
        public long PartId { get; set; }
        public string PartClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal? QtyToOrder { get; set; }
        public string UnitofMeasure { get; set; }
        public decimal? LastPurchaseCost { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public int TotalCount { get; set; }
    }
}