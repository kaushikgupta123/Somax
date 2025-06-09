using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Parts
{
    public class ReviewSiteModel
    {
        public long PartId { get; set; }
        public string SiteName { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal QtyOnHand { get; set; }
        public decimal QtyOnOrder { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public decimal LastPurchaseCost { get; set; }
        public string LastPurchaseVendor { get; set; }
        public bool InactiveFlag { get; set; }
        public bool RequestTransferStatus { get; set; }
    }
}