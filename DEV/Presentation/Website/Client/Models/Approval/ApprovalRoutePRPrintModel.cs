using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Approval
{
    public class ApprovalRoutePRPrintModel
    {
        public string ClientLookupId { get; set; }
        public string VendorName { get; set; }
        public string Date { get; set; }
        public string Comments { get; set; }
    }
}