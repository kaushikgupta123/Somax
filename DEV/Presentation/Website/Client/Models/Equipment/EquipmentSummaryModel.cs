namespace Client.Models
{
    public class EquipmentSummaryModel
    {
        public string Equipment_ClientLookupId { get; set; }
        public string EquipmentName { get; set; }
        public string ImageUrl { get; set; }
        public long OpenWorkOrders { get; set; }
        public long WorkRequests { get; set; }
        public long OverduePms { get; set; }
        //V2-636
        public bool RemoveFromService { get; set; }
        public string Status { get; set; }
        public bool ClientOnPremise { get; set; }
    }
}