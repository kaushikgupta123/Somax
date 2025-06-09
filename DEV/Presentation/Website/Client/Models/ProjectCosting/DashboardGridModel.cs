namespace Client.Models.ProjectCosting
{
    public class DashboardGridModel
    {
        public decimal Budget { get; set; }
        public decimal? MaterialCost { get; set; }
        public decimal? LaborCost { get; set; }
        public decimal PurchasingCost { get; set; }
        public decimal Spent { get; set; }
        public decimal Remaining { get; set; }
        public decimal SpentPercentage { get; set; }
        public decimal RemainingPercentage { get; set; }
    }
}