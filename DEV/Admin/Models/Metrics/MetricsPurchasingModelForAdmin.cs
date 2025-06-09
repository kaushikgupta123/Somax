namespace Admin.Models.Metrics
{
    public class MetricsPurchasingModelForAdmin
    {
        public string ClientName { get; set; }
        public string SiteName { get; set; }
        public decimal PurchaseOrdersCreated { get; set; }
        public decimal PurchaseOrdersCompleted { get; set; }
        public decimal ReceivedAmount { get; set; }
    }
}