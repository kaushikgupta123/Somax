namespace Client.Models.Invoice
{
    public class InvoiceGridListModel
    {
        
        public long InvoiceMatchHeaderId { get; set; }
        public long POReceiptItemID { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal UnitCost { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Description { get; set; }  
        public long AccountId { get; set; }
    }
}