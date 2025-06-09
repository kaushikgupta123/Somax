using System;

namespace Client.Models.Parts
{
    public class PartsReceiptPrintModel
    {
        public string POClientLookupId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal UnitCost { get; set; }
    }
}