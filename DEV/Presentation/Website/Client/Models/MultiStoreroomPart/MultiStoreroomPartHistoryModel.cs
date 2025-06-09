using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartHistoryModel
    {
        public long ClientId { get; set; }
        public long AccountId { get; set; }
        public string ChargeType_Primary { get; set; }
        public long ChargeToId_Primary { get; set; }
        public long RequestorId { get; set; }
        public string Comments { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public long PerformById { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal TransactionQuantity { get; set; }
        public string TransactionType { get; set; }
        public string UnitofMeasure { get; set; }
        public string Account_ClientLookupId { get; set; }
        public string Account_Name { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string Requestor_Name { get; set; }
        public string PerformBy_Name { get; set; }
        public string PurchaseOrder_ClientLookupId { get; set; }
        public string Vendor_ClientLookupId { get; set; }
        public string Vendor_Name { get; set; }
        public long PartId { get; set; }
        public bool Receipts { get; set; }
        public bool Reversals { get; set; }
        public string DateRange { get; set; }
        public string Storeroom { get; set; } //V2-1033
        public IEnumerable<SelectListItem> HistoryDateList { get; set; }
        public string HistoryDaterange { get; set; }

        public string PersonnelInitial { get; set; }
    }
}