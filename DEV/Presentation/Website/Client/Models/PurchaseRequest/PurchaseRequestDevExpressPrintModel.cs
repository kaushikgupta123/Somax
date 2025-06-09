using Client.Models.Work_Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseRequest
{
    public class PurchaseRequestDevExpressPrintModel
    {
        public PurchaseRequestDevExpressPrintModel()
        {
            LineItemDevExpressPrintModelList = new List<PRLineItemDevExpressPrintModel>();
            CommentsDevExpressPrintModelList = new List<CommentsDevExpressPrintModel>();
        }
        #region Header
        public string AzureImageUrl { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ClientlookupId { get; set; }
        #region Localizations
        public string GlobalPurchaseRequest { get; set; }
        public string spnCreatedBy { get; set; }
        public string globalCreateDate { get; set; }
        #endregion
        #endregion
        #region Vendor/PR Header
        public long PRHeaderUDF_PRId { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress1{ get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorAddress3 { get; set; }
        public string VendorCity { get; set; }
        public string VendorState { get; set; }
        public string VendorZip { get; set; }
        public string VendorCountry { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZip { get; set; }
        public string ShipToCountry { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
        public string BillToAddress3 { get; set; }
        public string BillToCity { get; set; }
        public string BillToState { get; set; }
        public string BillToZip { get; set; }
        public string BillToCountry { get; set; }
        #region Localizations
        public string spnVendor { get; set; }
        public string GlobalShipTo { get; set; }
        public string spnPRBillTo { get; set; }
        #endregion
        #endregion
        #region Purchase Request Header UIC
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
        #region Reason
        public string Reason { get; set; }
        #region Localizations
        public string GlobalReason { get; set; }
        #endregion
        #endregion
        #region Line Item
        public List<PRLineItemDevExpressPrintModel> LineItemDevExpressPrintModelList { get; set; }
        #endregion
        #region Comments
        public List<CommentsDevExpressPrintModel> CommentsDevExpressPrintModelList { get; set; }
        #endregion
        #region Settings Column
        public bool PRUIC { get; set; }
        public bool PRLine2 { get; set; }
        public bool PRLIUIC { get; set; }
        public bool PRComments { get; set; }
        #endregion
        public bool OnPremise { get; set; }
    }
}