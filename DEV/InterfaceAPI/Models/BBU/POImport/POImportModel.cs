using InterfaceAPI.Models.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.POImport
{
    public class POImportModel : LocalisationBaseVM
    {
        public POImportModel()
        {
            ClientId = 0;
            SiteId = 0;
            ClientLookupId = "";
            POImport2Id = 0;
            PurchaseOrder = "";
            ExPurchaseOrderId = 0;
            PurchaseRequest = "";
            PurchaseRequestId = 0;
            ExRequest = "";
            Required = DateTime.MinValue;
            POCreateDate = DateTime.MinValue;
            Currency = "";
            ExVendor = "";
            ExVendorId = 0;
            ExVendorSiteId = 0;
            PRLineItemId = 0;
            LineNumber = 0;
            ExPurchaseOrderLineId = 0;
            ExPart = "";
            ExPartId = 0;
            Description = "";
            Category = "";
            PurchaseQuantity = 0;
            PurchaseUOM = "";
            UnitCost = 0;
            UOMConversion = 0;
            WorkOrder = "";
            LineStatus = "";
            ErrorMessage = "";
            LastProcess= DateTime.MinValue;
            Revision = 0;

            VendorAddress1 = "";
            VendorAddress2 = "";
            VendorAddress3 = "";
            VendorAddressCity = "";
            VendorAddressState = "";
            VendorAddressPostCode = "";
            VendorAddressCountry = "";
            VendorName = "";
            MessageToVendor = "";
            VendorPhoneNumber = "";

            BillToName = "";
            BillToAddress1 = "";
            BillToAddress2 = "";
            BillToAddress3 = "";
            BillToAddressCity = "";
            BillToAddressState = "";
            BillToAddressPostCode = "";
            BillToAddressCountry = "";
            ShipToAddress1 = "";
            ShipToAddress2 = "";
            ShipToAddress3 = "";
            ShipToAddressCity = "";
            ShipToAddressState = "";
            ShipToAddressPostCode = "";
            ShipToAddressCountry = "";
            PaymentTerms = "";
            ExpenseAccount = "";
            CreateDate = "";
            FOB = "";
            Carrier = "";
            Creator_PersonnelName = "";
            PersonnelEmail = "";
            SiteName = "";
            PrintDate = "";
            Logo = "";
            PurchaseTerms = "";
            poImportLineItemsModel = null;
        }
        public long ClientId { get; set; }
        public long POImport2Id { get; set; }
        public long SiteId { get; set; }
        public string ClientLookupId { get; set; }
        public string PurchaseOrder { get; set; }
        public long ExPurchaseOrderId { get; set; }
        public string PurchaseRequest { get; set; }
        public long PurchaseRequestId { get; set; }
        public string ExRequest { get; set; }
        public DateTime? Required { get; set; }
        public DateTime? POCreateDate { get; set; }
        public string Currency { get; set; }
        public string ExVendor { get; set; }
        public long ExVendorId { get; set; }
        public long ExVendorSiteId { get; set; }
        public long PRLineItemId { get; set; }
        public int LineNumber { get; set; }
        public long ExPurchaseOrderLineId { get; set; }
        public string ExPart { get; set; }
        public long ExPartId { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal PurchaseQuantity { get; set; }
        public string PurchaseUOM { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UOMConversion { get; set; }
        public string WorkOrder { get; set; }
        public string LineStatus { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? LastProcess { get; set; }
        public int Revision { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
       
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
       
        public string PaymentTerms { get; set; }
        public string ExpenseAccount { get; set; }

        public string VendorAddressCountry { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress1 { get; set; }
        public string MessageToVendor { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorAddress3 { get; set; }
        public string VendorAddressCity { get; set; }
        public string VendorAddressState { get; set; }
        public string VendorAddressPostCode { get; set; }
        public string VendorPhoneNumber { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress3 { get; set; }
        public string BillToAddressCity { get; set; }
        public string BillToAddressState { get; set; }
        public string BillToAddressPostCode { get; set; }
        public string BillToAddressCountry { get; set; }
        public string CreateDate { get; set; }
        public string FOB { get; set; }
        public string Carrier { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string PersonnelEmail { get; set; }
        public string SiteName { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToAddressCity { get; set; }
        public string ShipToAddressState { get; set; }
        public string ShipToAddressPostCode { get; set; }
        public string ShipToAddressCountry { get; set; }
        public int UpdateIndex { get; set; }
        public string PrintDate { get; set; }
        public string Logo { get; set; }
        public string PurchaseTerms { get; set; }
        public Int64 PurchaseOrderId { get; set; }
        public List<POImportLineItemsModel> poImportLineItemsModel { get; set; }
        
    }

    public class POImportLineItemsModel
    {
        public POImportLineItemsModel()
        {
            PartClientLookupId = "";
            Description = "";
            PartNumber = "";
            PurchaseQuantity = "";
            PurchaseUOM = "";
            PurchaseCost = "";
            LineTotal = "";
            Category = "";
            EstimatedDelivery = "";
            LineNumber = 0;

        }

        public string PartClientLookupId { get; set; }
        public string Description { get; set; }
        public string PartNumber { get; set; }
        public string PurchaseQuantity { get; set; }
        public string PurchaseUOM { get; set; }
        public string PurchaseCost { get; set; }
        public string LineTotal { get; set; }
        public string Category { get; set; }
        public string EstimatedDelivery { get; set; }
        public int LineNumber { get; set; }

    }
}