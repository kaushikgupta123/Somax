namespace Client.Models
{
    public class MultiStoreroomPartVendorGridModel
    {
        public string Vendor_ClientLookupId { get; set; }
        public string Vendor_Name { get; set; }
        public long ClientId { get; set; }
        public long Part_Vendor_XrefId { get; set; }
        public long PartId { get; set; }
        public long VendorId { get; set; }
        public bool PreferredVendor { get; set; }
        public string CatalogNumber { get; set; }
        public decimal IssueOrder { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }
        public int OrderQuantity { get; set; }
        public string OrderUnit { get; set; }
        public decimal Price { get; set; }
        public int UpdateIndex { get; set; }
        public string PartVendorSecurity { get; set; }
        public bool UOMConvRequired { get; set; }
    }
}