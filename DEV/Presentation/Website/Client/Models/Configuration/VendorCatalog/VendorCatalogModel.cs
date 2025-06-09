using System;

namespace Client.Models.Configuration.VendorCatalog
{
    public class VendorCatalogModel
    {
        public long ClientId{get;set;}
        public long PartMasterId{get;set;}
        public string ClientLookupId{get;set;}
        public bool OEMPart{get;set;}
        public long EXPartId{get;set;}
        public string EXAltPartId1{get;set;}
        public string EXAltPartId2{get;set;}
        public string EXAltPartId3{get;set;}
        public Guid ExUniqueId{get;set;}
        public bool InactiveFlag{get;set;}
        public string LongDescription{get;set;}
        public string Manufacturer{get;set;}
        public string ManufacturerId{get;set;}
        public string ShortDescription{get;set;}
        public decimal UnitCost{get;set;}
        public string UnitOfMeasure{get;set;}
        public string Category{get;set;}
        public string UPCCode{get;set;}
        public string ImageURL{get;set;}
        public bool SXPart{get;set;}
        public int UpdateIndex{get;set;}
        public string VCI_PartNumber { get; set; }
        public string CM_Description { get; set; }
        public string VendorName { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VI_PurchaseUOM { get; set; }
    }
}