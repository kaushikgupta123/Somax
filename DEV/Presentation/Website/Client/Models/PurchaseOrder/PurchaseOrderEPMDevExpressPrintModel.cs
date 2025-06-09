using Client.Models.PurchaseOrder;

using DevExpress.DataAccess.ObjectBinding;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Client.Models.PurchaseOrder
{
    public class PurchaseOrderEPMDevExpressPrintModel
    {
        public PurchaseOrderEPMDevExpressPrintModel()
        {
            POLineItemEPMDevExpressPrintModelList = new List<POLineItemEPMDevExpressPrintModel>();
            POCommentsEPMDevExpressPrintModelList = new List<POCommentsEPMDevExpressPrintModel>();

        }
        #region Vendor/PO Header


        public string eastPenn_immagePath { get; set; }
        public long PurchaseOrderId { get; set; }
        public string ClientlookupId { get; set; }
        public string CreateDate { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public string SDate { get; set; }
        public long StoreroomId { get; set; }
        public string CompanyAddress_Line1 { get; set; }
        public string CompanyAddress_Line2 { get; set; }
        public string CompanyAddress_Line3 { get; set; }
        public long ShipTo { get; set; }
        public string ShipToClientLookupId { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToAddressCity { get; set; }
        public string ShipToAddressState { get; set; }
        public string ShipToAddressPostCode { get; set; }
        public string ShipToAddressCountry { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress1 { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorAddress3 { get; set; }
        public string VendorAddressCity { get; set; }
        public string VendorAddressState { get; set; }
        public string VendorAddressPostCode { get; set; }
        public string VendorAddressCountry { get; set; }
        public string SiteName { get; set; }
        public string SiteBillToName { get; set; }
        public string SiteAddress1 { get; set; }
        public string SiteAddress2 { get; set; }
        public string SiteAddress3 { get; set; }
        public string SiteAddressCity { get; set; }
        public string SiteAddressState { get; set; }
        public string SiteAddressPostCode { get; set; }
        public string SiteAddressCountry { get; set; }
        public string Required { get; set; }
        public string Terms { get; set; }
        public string Carrier { get; set; }

        #region Localizations

        public string GlobalPurchaseOrder { get; set; }
        public string spnAcknowledgement { get; set; }
        public string spnPoTerms { get; set; }
        public string spnRequired { get; set; }
        public string spnPoCarrier { get; set; }
        public string spnDeliveryTerms { get; set; }
        public string globalSpnTo { get; set; }
        public string globalShipTo { get; set; }
        public string globalOrderPlaced { get; set; }
        public string globalNumber { get; set; }
        public string globalStatus { get; set; }
        public string globalBlanketOrderRef { get; set; }
        public string spnPhoneNumber { get; set; }
        public string spnFax { get; set; }
        public string globalInvoiceTo { get; set; }

        #endregion
        #endregion
        #region Line Item
        public List<POLineItemEPMDevExpressPrintModel> POLineItemEPMDevExpressPrintModelList { get; set; }
        #region Localizations
        #endregion
        #endregion
        #region Comments
        public List<POCommentsEPMDevExpressPrintModel> POCommentsEPMDevExpressPrintModelList { get; set; }
        #region Localizations
        #endregion
        #endregion
    }
}