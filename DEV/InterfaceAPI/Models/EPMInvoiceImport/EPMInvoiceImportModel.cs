using System;
using System.Collections.Generic;

namespace InterfaceAPI.Models.EPMInvoiceImport
{
  
        public class EPMInvoiceImportModel
        {
            public EPMInvoiceImportModel()
            {
                PONumber = "";
                PODate = "";
                TranType = "";
                CarrierName = "";
                ShipToAttn = "";
                ShipToLocation = "";
                ShipToName = "";
                ShipToStreet1 = "";
                ShipToStreet2 = "";
                ShipToStreet3 = "";
                ShipToCity = "";
                ShipToState = "";
                ShipToPostalCode = "";
                ShipToCountry = "";
                VendorNumber = "";
                VendorName = "";
                VendorStreet1 = "";
                VendorStreet2 = "";
                VendorStreet3 = "";
                VendorCity = "";
                VendorState = "";
                VendorPostalCode = "";
                VendorCountry = "";
                BuyerName = "";
                BuyerPhoneNumber = "";
                POTotalCost = 0;
            }
            // Header Section

            public string HeaderQualifier { get; set; } = "H";
            public long SiteId { get; set; }
            public long ClientId { get; set; }
            public string InvoiceDate { get; set; }
            public string InvoiceNumber { get; set; }
            public string PONumber { get; set; }
            public string PODate { get; set; }
            public string TranType { get; set; }
            public string CarrierDetails { get; set; } = string.Empty;
            public string CarrierName { get; set; }
            public string ShipToAttn { get; set; }
            public string ShipToLocation { get; set; }
            public string ShipToName { get; set; }
            public string ShipToStreet1 { get; set; }
            public string ShipToStreet2 { get; set; }
            public string ShipToStreet3 { get; set; }
            public string ShipToCity { get; set; }
            public string ShipToState { get; set; }
            public string ShipToPostalCode { get; set; }
            public string ShipToCountry { get; set; }
            public string VendorNumber { get; set; }
            public string VendorName { get; set; }
            public string VendorStreet1 { get; set; }
            public string VendorStreet2 { get; set; }
            public string VendorStreet3 { get; set; }
            public string VendorCity { get; set; }
            public string VendorState { get; set; }
            public string VendorPostalCode { get; set; }
            public string VendorCountry { get; set; }
            public string RemitName { get; set; }
            public string RemitAddress1 { get; set; }
            public string RemitAddress2 { get; set; }
            public string RemitAddress3 { get; set; }
            public string RemitCity { get; set; }
            public string RemitState { get; set; }
            public string RemitZip { get; set; }
            public string RemitCountry { get; set; }
            public string BuyerName { get; set; }
            public string BuyerPhoneNumber { get; set; }
            public string BuyerFaxNumber { get; set; }
            public string ShipDate { get; set; }
            public string TermsNetDueDate { get; set; }
            public decimal POTotalCost { get; set; }

            // Header Comment Section
            public List<HeaderComment> HeaderComment { get; set; } = new List<HeaderComment>();

            // Items Section
            public List<Item> Items { get; set; } = new List<Item>();

            // Items Summary Section
            public List<ItemSummary> ItemsSummaries { get; set; } = new List<ItemSummary>();
        }

        public class HeaderComment
        {
            public HeaderComment()
            {
                PONumber = "";
                Notes = "";
            }
            public string Qualifier { get; set; } = "HC";
            public string PONumber { get; set; }
            public string Notes { get; set; }
            public string Notes2 { get; set; } = "";
        }
        public class Item
        {
            public Item()
            {
                PONumber = "";
                LineNumber = 0;
                Quantity = 0;
                UnitOfMeasurement = "";
                UnitPrice = 0;
                BuyerPartNumber = "";
                DeliveryDate = "";
                RequiredDate = "";
            }
            public string Qualifier { get; set; } = "I";
            public string PONumber { get; set; }
            public int LineNumber { get; set; }
            public decimal Quantity { get; set; }
            public string UnitOfMeasurement { get; set; }
            public decimal UnitPrice { get; set; }
            public string BuyerPartNumber { get; set; }
            public string VendorPartNumber { get; set; } = string.Empty;
            public string DeliveryDate { get; set; }
            public string RequiredDate { get; set; }

            // Items Comment Section
            public List<ItemComment> ItemsComments { get; set; } = new List<ItemComment>();
        }

        public class ItemComment
        {
            public ItemComment()
            {
                PONumber = "";
                Description = "";
            }
            public string Qualifier { get; set; } = "IC";
            public string PONumber { get; set; }
            public string Description { get; set; }
        }

        public class ItemSummary
        {
            public ItemSummary()
            {
                PONumber = "";
                LineNumber = 0;
                Quantity = 0;
                UnitOfMeasurement = "";
                DeliveryDate = "";
            }
            public string Qualifier { get; set; } = "S";
            public string PONumber { get; set; }
            public int LineNumber { get; set; }
            public decimal Quantity { get; set; }
            public string UnitOfMeasurement { get; set; }
            public string DeliveryDate { get; set; }
        }
    
}
