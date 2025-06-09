using System.ComponentModel.DataAnnotations;
using System;

namespace Client.Models.PurchaseOrder
{
    public class POLineItemEPMDevExpressPrintModel
    {
        public int LineNumber { get; set; }
        public string EPMPart { get; set; }
        public string SubPart { get; set; }
        public string PartDescription { get; set; }
        public decimal OrderQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerID { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Extension { get; set; }
        public string Status_Display { get; set; }
        public string ChargeToClientLookupId { get; set; }
        #region Localizations
        public string globalLineItems { get; set; }
        public string globalLine { get; set; }
        public string globalEPMPart { get; set; }
        public string globalSUPPart { get; set; }
        public string spnPartDescription { get; set; }
        public String EstimatedDelivery { get; set; }
        
        public string globalUOM { get; set; }
        public string spnQty { get; set; }
        public string spnPrice { get; set; }
        public string globalExtension { get; set; }
        public string GlobalStatus { get; set; }
        public string GlobalChargeTo { get; set; }
        public string GlobalGrandTotal { get; set; }
        
        #endregion

    }
}