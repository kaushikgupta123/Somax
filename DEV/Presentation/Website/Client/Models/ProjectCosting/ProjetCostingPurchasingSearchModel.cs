namespace Client.Models.ProjectCosting
{
    public class ProjetCostingPurchasingSearchModel
    {
        public long ProjectId { get; set; }
        public string ClientLookupId { get; set; }
        public int Line { get; set; }
        public string PartID { get; set; }
        public string Description { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? TotalCost { get; set; }
        public string Status { get; set; }
        public string Buyer { get; set; }
        public string CompleteDate { get; set; }
        public int TotalCount { get; set; }
    }
}