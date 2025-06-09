using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.AutoPRGeneration
{
    public class AutoPRGenerationPrintModel
    {
        public string PartClientLookupId { get; set; }
        public string Description { get; set; }
        public string QtyToOrder { get; set; }
        public string UnitofMeasure { get; set; }
        public string LastPurchaseCost { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
    }
}