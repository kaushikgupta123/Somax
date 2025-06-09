using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class PurchaseOrderLookupModel
    {
        public long PurchaseOrderId { get; set; }
        public string POClientLookupId { get; set; }
        public string Status { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public long TotalCount { get; set; }

    }
}