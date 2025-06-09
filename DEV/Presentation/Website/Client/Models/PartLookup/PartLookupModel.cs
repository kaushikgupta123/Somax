using System;
namespace Client.Models.PartLookup
{
    public class PartLookupModel
    {
        public long PartId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string ImageUrl { get; set; }
        public int TotalCount { get; set; }
        public bool InVendorCatalog { get; set; }
        public string VendorPartNumber { get; set; }
        public decimal QtyMaximum { get; set; }//V2-424
        public decimal QtyOnHand { get; set; }//V2-424
        public decimal QtyReorderLevel { get; set; }//V2-424
        //V2-553
        public string IssueUOM { get; set; }
        public decimal UOMConversion { get; set; }
        public bool UOMConvRequired { get; set; }
        public int VC_Count { get; set; }
        public long VendorId { get; set; }
        public string PurchaseUOM { get; set; }
        public decimal IssueOrder { get; set; }
        public long VendorCatalogItemId { get; set; }
        public DateTime RequiredDate { get; set; }
        public string IssueUnit { get; set; }
        //V2-690
        public long PartCategoryMasterId { get; set; }
        public string VendorClientlookupId { get; set; }

        public long indexid { get; set; }
        //V2-732
        public long StoreroomId { get; set; }
        public long PartStoreroomId { get; set; }
        //V2-932
        public decimal OnOrderQty { get; set; }
        public decimal OnRequestQTY { get; set; }
        //V2-1068
        public long AccountId { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}