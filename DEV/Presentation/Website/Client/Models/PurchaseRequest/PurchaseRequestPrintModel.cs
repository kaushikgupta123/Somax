using System;

namespace Client.Models.PurchaseRequest
{
    public class PurchaseRequestPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string PurchaseOrderClientLookupId { get; set; }
        public string Processed_PersonnelName { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}