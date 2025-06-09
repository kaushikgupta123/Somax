using System;

namespace Client.Models.Invoice
{
    public class InvoicePrintModel
    {
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string POClientLookupId { get; set; }

    }
}