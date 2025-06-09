

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartSummaryModel
    {
        public string PartImageUrl { get; set; }
        public string ClientLookupId { get; set; }
        public string IssueUnit { get; set; }
        public decimal? AppliedCost { get; set; }
        public string Description { get; set; }
        public bool ClientOnPremise { get; set; }
        public bool InactiveFlag { get; set; }
        //V2-1025
        public decimal TotalOnHand { get; set; }
        public decimal TotalOnRequest { get; set; }
        public decimal TotalOnOrder { get; set; }
        public string DefaultStoreroom { get; set; }
        public string StockType { get; set; }
    }
}