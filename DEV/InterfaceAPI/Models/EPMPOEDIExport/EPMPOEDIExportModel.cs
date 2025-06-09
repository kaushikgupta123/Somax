using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.EPMPOEDIExport
{
    public class EPMPOEDIExportModel
    {
        public EPMPOEDIExportModel()
        {
            PONumber = "";
            PODate = "";
            TermsDescription = "";
            CarrierName = "";
            ShipToAttn = "";
            ShipToLocation = "";
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
        public string PONumber { get; set; }
        public string PODate { get; set; }
        public string TermsDescription { get; set; }
        public string CarrierDetails { get; set; } = string.Empty;
        public string CarrierName { get; set; }
        public string ShipToAttn { get; set; }
        public string ShipToLocation { get; set; }
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
        public string BuyerName { get; set; }
        public string BuyerPhoneNumber { get; set; }
        public string BuyerFaxNumber { get; set; } = "6106826361";
        public decimal POTotalCost { get; set; }

        // Header Comment Section
        public List<HeaderComment> HeaderComment { get; set; } = new List<HeaderComment>();

        // Items Section
        public List<Item> Items { get; set; } = new List<Item>();
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

        // Items Summary Section
        public ItemSummary ItemsSummaries { get; set; } = new ItemSummary();
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