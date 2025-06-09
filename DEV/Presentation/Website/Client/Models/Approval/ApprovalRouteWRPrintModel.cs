using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Approval
{
    public class ApprovalRouteWRPrintModel
    {
        public string ClientLookupId { get; set; }
        public string ChargeTo { get; set; }
        public string ChargeToName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public string Comments { get; set; }
    }
}