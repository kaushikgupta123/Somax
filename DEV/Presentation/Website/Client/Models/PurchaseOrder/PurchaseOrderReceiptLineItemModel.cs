namespace Client.Models.PurchaseOrder
{
    public class PurchaseOrderReceiptLineItemModel
    {
        public long PurchaseOrderLineItemId { get; set; }
        public long PurchaseOrderId { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityToDate { get; set; }
        public decimal CurrentAverageCost { get; set; }
        public decimal CurrentAppliedCost { get; set; }
        public decimal CurrentOnHandQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public long AccountId { get; set; }
        public long Creator_PersonnelId { get; set; }
        public long PartId { get; set; }
        public long StoreroomId { get; set; }
        public string Description { get; set; }
        public string StockType { get; set; }
        public string UnitOfMeasure { get; set; }
        public long ChargeToId { get; set; }
        public string ChargeType { get; set; }
        public long PartStoreroomId { get; set; }
        public long POReceiptHeaderId { get; set; }

        public decimal UOMConversion { get; set; }
        public string PurchaseUOM { get; set; }
        public long EstimatedCostsId { get; set; } //V2-1124
    }
}