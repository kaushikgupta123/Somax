using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Approval
{
    public class ApprovalRouteMRPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime? Date { get; set; }
        public string Comments { get; set; }
    }
}