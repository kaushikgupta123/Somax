using System;

namespace Client.Models.Approval
{
    internal class PRApprovalSearchModel
    {
        public long ApprovalGroupId { get; set; }
        public long ApprovalRouteId { get; set; }
        public long PurchaseRequestId { get; set; }
        public string ClientLookupId { get; set; }
        public string VendorName { get; set; }
        public string Comments { get; set; }
        public string Date { get; set; }
        public int TotalCount { get; set; }
    }
}