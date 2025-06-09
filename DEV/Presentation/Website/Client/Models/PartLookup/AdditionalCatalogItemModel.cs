using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartLookup
{
    public class AdditionalCatalogItemModel
    {
        public long PartId { get; set; }
        public long VendorId { get; set; }
        public string PartClientLookupId { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public string VendorPartNumber { get; set; }
        public decimal UnitCost { get; set; }
        public string PurchaseUOM { get; set; }
        public string Description { get; set; }
        public long PartStoreroomId { get; set; }
        public string IssueUnit { get; set; }
        public long VendorCatalogItemid { get; set; }
    }
}