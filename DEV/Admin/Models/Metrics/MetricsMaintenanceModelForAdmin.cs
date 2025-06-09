namespace Admin.Models.Metrics
{
    public class MetricsMaintenanceModelForAdmin
    {
        public string ClientName { get; set; }
        public string SiteName { get; set; }
        public decimal WorkOrdersCreated { get; set; }
        public decimal WorkOrdersCompleted { get; set; }
        public decimal LaborHours { get; set; }
    }
}