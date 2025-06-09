using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseRequest
{
    public class PurchaseRequestPunchOutModel
    {
        public string VendorClientLookupId { get; set; }
        public long? VendorId { get; set; }

        public string VendorName { get; set; }
    }
}