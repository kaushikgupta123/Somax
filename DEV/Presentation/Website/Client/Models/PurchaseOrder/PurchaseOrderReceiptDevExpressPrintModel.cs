using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseOrder
{
    public class PurchaseOrderReceiptDevExpressPrintModel
    {
        public PurchaseOrderReceiptDevExpressPrintModel()
        {
            PurchaseOrderReceiptLineItemDevExpressPrintModelList = new List<PurchaseOrderReceiptLineItemDevExpressPrintModel>();
            POCommentsDevExpressPrintModelList = new List<POCommentsDevExpressPrintModel>();
        }
        #region Line Item
        public List<PurchaseOrderReceiptLineItemDevExpressPrintModel> PurchaseOrderReceiptLineItemDevExpressPrintModelList { get; set; }
        #region Localizations
        public string spnLineItems { get; set; }
        #endregion
        #endregion
        public long PurchaseOrderId { get; set; }
        #region Header
        
        public string AzureImageUrl { get; set; }
        public string ReceiveBy { get; set; }
        public string ReceiveDate { get; set; }
        public string ReceiptNumber { get; set; }
        public string ClientlookupId { get; set; }
        public string Comments { get; set; }
        #region Localizations
        public string GlobalPurchaseOrder { get; set; }
        public string GlobalReceiveBy { get; set; }
        public string globalReceiveDate { get; set; }
        public string globalReceiptNumber { get; set; }
        #endregion
        #endregion
        #region Vendor/PO Header

        public long POHeaderUDF_POId { get; set; }
        public string VendorName { get; set; }
        public string VendorEmailAddress { get; set; }
        public string VendorAddress1 { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorAddress3 { get; set; }
        public string VendorAddressCity { get; set; }
        public string VendorAddressState { get; set; }
        public string VendorAddressPostCode { get; set; }
        public string VendorAddressCountry { get; set; }
        public string VenndorAdresssCSPC { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress1 { get; set; }
        public string SiteAddress2 { get; set; }
        public string SiteAddress3 { get; set; }
        public string SiteAddressCity { get; set; }
        public string SiteAddressState { get; set; }
        public string SiteAddressPostCode { get; set; }
        public string SiteAddressCountry { get; set; }
        public string siteAdresssCSPC { get; set; }
        public string BillToName { get; set; }
        public string SiteBillToAddress1 { get; set; }
        public string SiteBillToAddress2 { get; set; }
        public string SiteBillToAddress3 { get; set; }
        public string SiteBillToAddressCity { get; set; }
        public string SiteBillToAddressState { get; set; }
        public string SiteBillToAddressPostCode { get; set; }
        public string SiteBillToAddressCountry { get; set; }
        public string sitebillAdresssCSPC { get; set; }
        public string PersonnelName { get; set; }
        public string Carrier { get; set; }
        public string Attention { get; set; }
        public string MessageToVendor { get; set; }
      
        public string PackingSlip { get; set; }
        public string FreightBill { get; set; }
        public string FreightAmount { get; set; }

        #region Localizations
        public string spnPurchaseOrderReceipt { get; set; }
        public string spnVendorPOHeader { get; set; }
        public string spnVendor { get; set; }
        public string GlobalShipTo { get; set; }
        public string spnPOBillTo { get; set; }
        public string spnPoCarrier { get; set; }
        public string spnPoAttention { get; set; }
        public string spnPoMessageToVendor { get; set; }
        public string spnReceiptNumber { get; set; }
        public string spnReccord { get; set; }
        public string spnReceivedBy { get; set; }
        public string spnPackingSlip { get; set; }
        public string spnFreightBill { get; set; }
        public string spnFreightAmt { get; set; }
        public string spnPoComments { get; set; }
        #endregion
        #endregion

        public bool PORHeader { get; set; }
        public bool PORLine2 { get; set; }
        public bool PORPrint { get; set; }
        public bool OnPremise { get; set; }

        #region V2-1011
        #region Purchase Order Header UIC
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
        public string spnPurchaseOrderHeaderUIC { get; set; }
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

        #region Comments
        public List<POCommentsDevExpressPrintModel> POCommentsDevExpressPrintModelList { get; set; }
        #region Localizations
        public string spnLogComment { get; set; }
        #endregion
        public bool PORUIC { get; set; }
        public bool PORComments { get; set; }
        #endregion
        #endregion
    }
}