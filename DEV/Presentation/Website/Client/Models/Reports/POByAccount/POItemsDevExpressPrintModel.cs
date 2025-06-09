using System.ComponentModel.DataAnnotations;
using System;

namespace Client.Models.Reports
{
    public class POItemsDevExpressPrintModel
    {
        #region Area 1
        public string ClientLookupId { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string CreateDate { get; set; }
        public string AccountClientLookupId { get; set; }
        public string AccountName { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public int LineNumber { get; set; }
        public string POLDescription { get; set; }
        public decimal OrderQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal RemainingQuantity { get; set; }
        public decimal ReceivedTotalCost { get; set; }
        public decimal RemainingCost { get; set; }
        public string LineItemStatus { get; set; }
        public string PartClientLookupId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReportDate { get; set; }
        #region Localizations
        public string spnPurchaseOrderByAccount { get; set; }
        public string GlobalAccount { get; set; }
        public string spnPurchaseOrder { get; set; }
        public string GlobalReason { get; set; }
        public string GlobalVendor { get; set; }
        public string GlobalVendorName { get; set; }
        public string GlobalStatus { get; set; }
        public string globalOrderDate { get; set; }
        public string spnPoLineNo { get; set; }
        public string spnPartID { get; set; }
        public string spnDescription { get; set; }
        public string spnPoOrderQty { get; set; }
        public string spnPdUOM { get; set; }
        public string globalLineCost { get; set; }
        public string spnQtyRec { get; set; }
        public string spnReceivedCost { get; set; }
        public string globalQtyRemaining { get; set; }
        public string globalRemainingCost { get; set; }
        public string globalTotalCostForAccount { get; set; }
        public string globalTotalRemaining { get; set; }
        public string spnTotalReceived { get; set; }
        public string globalGrandTotalofLineReceived { get; set; }
        public string globalGrandTotalofLineRemaining { get; set; }
        public string globalGrandTotalofLineTotal { get; set; }
        public string spnCreated { get; set; }
        public string globalthrough { get; set; }
        #endregion
        #endregion
    }
}