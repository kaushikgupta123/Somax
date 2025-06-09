using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseOrder
{
    public class PurchaseOrderReceiptLineItemDevExpressPrintModel
    {
        #region Area 1
        public int LineNumber { get; set; }
        public string PartClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal QuantityReceived { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GrandTotal { get; set; }
        public string Status { get; set; }
        public string Status_Display { get; set; }
       
        #region Localizations
        public string globalLineItems { get; set; }
        public string globalLine { get; set; }
        public string spnPartID { get; set; }
        public string spnDescription { get; set; }
        public string spnQtyRec { get; set; }
        public string spnUOM { get; set; }
        public string spnPrice { get; set; }
        public string spnTotal { get; set; }
        public string GlobalStatus { get; set; }
       
        #endregion
        #endregion
        #region Area 2
        public string AccountClientLookupId { get; set; }
        public string ChargeToClientLookupId { get; set; }
       
        #region Localizations
        public string GlobalAccount { get; set; }
        public string GlobalChargeTo { get; set; }
        public string spnPoGrandtotal { get; set; }
        #endregion
        public string GlobalGrandTotal { get; set; }
        #endregion
    }
}