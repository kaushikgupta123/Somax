using System;
using System.Collections.Generic;

namespace Client.Models.PurchaseOrder
{
    public class PurchaseOrderListPrintModel
    {
        public long PurchaseOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
    }
    public class MainPOPrintModel: POPrintModel
    {
        public List<PurchaseOrderListPrintModel> list { get; set; }
        public MainPOPrintModel()
        {
            this.list = new List<PurchaseOrderListPrintModel>();
        }
    }
    public class POPrintModel
    {
     
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Attention { get; set; }
        public string VendorPhoneNumber { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Reason { get; set; }
        public string Buyer_PersonnelName { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime? Required { get; set; } //V2-1171
    }

    public class PORPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Attention { get; set; }
    
    }
}