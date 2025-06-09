namespace Client.Models.InventoryReceipt
{
    public class GridPartReceiptList
    {
      
        public long? PartId { get; set; }
        public string PartClientLookupId { get; set; }
        public long? PerformedById { get; set; }
        public string PartUPCCode { get; set; }
        public string Description { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? PartAverageCost { get; set; }
        public decimal? ReceiptQuantity { get; set; }

       public string errorListInString { get; set; }
        public long? StoreroomId { get; set; }
        public string StoreroomName { get; set; }

    }
}