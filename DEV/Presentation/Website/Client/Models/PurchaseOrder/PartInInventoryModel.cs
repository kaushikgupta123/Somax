namespace Client.Models.PurchaseOrder
{
    public class PartInInventoryModel
    {
        public long PurchaseOrderId { get; set; }
        public string PurchaserequestClientLookupId { get; set; }
        public long PartId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }
        public decimal Quantity { get; set; }
    }
}