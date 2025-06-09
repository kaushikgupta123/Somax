namespace Client.Models.Work_Order
{
    public class ActivityLogCostModel
    {
        public decimal TotalCost { get; set; }
        public decimal PartCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal OtherCost { get; set; }
        public decimal PartCostPercentage { get; set; }
        public decimal LaborCostPercentage { get; set; }
        public decimal OtherCostPercentage { get; set; }

    }
}