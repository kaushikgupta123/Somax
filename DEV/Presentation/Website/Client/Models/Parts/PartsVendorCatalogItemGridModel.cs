namespace Client.Models
{
    public class PartsVendorCatalogItemGridModel
    {
        public long ClientId { get; set; }
        public long VendorCatalogItemId { get; set; }
        public string Vendor_Name { get; set; }
        public decimal UnitCost { get; set; }
        public string PurchaseUOM { get; set; }
        public int TotalCount { get; set; }
    }
}