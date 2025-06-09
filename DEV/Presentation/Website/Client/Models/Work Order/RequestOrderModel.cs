using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Work_Order
{
    public class RequestOrderModel
    {
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public string Vendor { get; set; }
        public string VendorName { get; set; }
        public DateTime Created { get; set; }
    }
}