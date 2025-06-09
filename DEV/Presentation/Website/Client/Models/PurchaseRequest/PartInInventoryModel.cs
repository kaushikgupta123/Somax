namespace Client.Models.PurchaseRequest
{
    public class PartInInventoryModel
    {
        public long PurchaseRequestId { get; set; }
        public string PurchaserequestClientLookupId { get; set; }
        public long PartId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public decimal Quantity { get; set; }        
    }
}