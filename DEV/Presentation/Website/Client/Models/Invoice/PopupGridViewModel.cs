using System;

namespace Client.Models.Invoice
{
    public class PopupGridViewModel
    {
        public long POReceiptItemId { get; set; }
        public string POClientLookupId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string PartClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal? QuantityReceived { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalCost { get; set; }
        public long AccountId { get; set; }


    }
}