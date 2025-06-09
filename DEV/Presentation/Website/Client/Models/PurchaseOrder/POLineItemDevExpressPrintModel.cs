using System.ComponentModel.DataAnnotations;
using System;

namespace Client.Models.PurchaseOrder
{
    public class POLineItemDevExpressPrintModel
    {
        public long POLineUDF_POLIId { get; set; }
        public long POLineUDF_POId { get; set; }
        #region Area 1
        public int LineNumber { get; set; }
        public string PartClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal OrderQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
        public string Status_Display { get; set; }
        public string ChargeToClientLookupId { get; set; }
        #region Localizations
        public string globalLineItems { get; set; }
        public string globalLine { get; set; }
        public string spnPartID { get; set; }
        public string spnDescription { get; set; }
        public string spnQty { get; set; }
        public string spnUOM { get; set; }
        public string spnPrice { get; set; }
        public string spnTotal { get; set; }
        public string GlobalStatus { get; set; }
        public string GlobalChargeTo { get; set; }
        #endregion
        #endregion
        #region Area 2
        public string AccountClientLookupId { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerID { get; set; }
        #region Localizations
        public string GlobalAccount { get; set; }
        public string GlobalManufacturer { get; set; }
        public string GlobalManufacturerID { get; set; }

        public string spnPoGrandtotal { get; set; }
        #endregion
        #endregion
        #region Area 3
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string Date3 { get; set; }
        public string Date4 { get; set; }
        public string Bit1 { get; set; }
        public string Bit2 { get; set; }
        public string Bit3 { get; set; }
        public string Bit4 { get; set; }
        public decimal Numeric1 { get; set; }
        public decimal Numeric2 { get; set; }
        public decimal Numeric3 { get; set; }
        public decimal Numeric4 { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public string Select4 { get; set; }
        #region Localizations
        public string Text1Label { get; set; }
        public string Text2Label { get; set; }
        public string Text3Label { get; set; }
        public string Text4Label { get; set; }
        public string Date1Label { get; set; }
        public string Date2Label { get; set; }
        public string Date3Label { get; set; }
        public string Date4Label { get; set; }
        public string Bit1Label { get; set; }
        public string Bit2Label { get; set; }
        public string Bit3Label { get; set; }
        public string Bit4Label { get; set; }
        public string Numeric1Label { get; set; }
        public string Numeric2Label { get; set; }
        public string Numeric3Label { get; set; }
        public string Numeric4Label { get; set; }
        public string Select1Label { get; set; }
        public string Select2Label { get; set; }
        public string Select3Label { get; set; }
        public string Select4Label { get; set; }
        #endregion
        #endregion
    }
}