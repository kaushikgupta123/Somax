using System;

namespace Client.Models.PartTransfer
{
    public class PartTransferPrintModel
    {
        public long PartTransferId { get; set; }        
        public string RequestSite_Name { get; set; }
        public string RequestPart_ClientLookupId { get; set; }
        public decimal? Quantity { get; set; }
        public string Status { get; set; }
        public string RequestPart_Description { get; set; }
        public string IssueSite_Name { get; set; }
        public string IssuePart_ClientLookupId { get; set; }
        public string Reason { get; set; }
        public string LastEvent { get; set; }
        public DateTime? LastEventDate { get; set; }
    }
}