using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartsHistoryPrintModel
    {
        public string TransactionType { get; set; }
        public string Storeroom { get; set; }
        public string Requestor_Name { get; set; }
        public string PerformBy_Name { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal TransactionQuantity { get; set; }
        public decimal Cost { get; set; }
        public string ChargeType_Primary { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string Account_ClientLookupId { get; set; }
        public string PurchaseOrder_ClientLookupId { get; set; }
        public string Vendor_ClientLookupId { get; set; }
        public string Vendor_Name { get; set; }
    }
}