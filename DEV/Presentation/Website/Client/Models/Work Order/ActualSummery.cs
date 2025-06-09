namespace Client.Models.Work_Order
{
    public class ActualSummery
    {
        public decimal TotalPartCost { get; set; }
        public decimal TotalCraftCost { get; set; }
        public decimal TotalExternalCost { get; set; }
        public decimal TotalInternalCost { get; set; }
        public decimal TotalSummeryCost { get; set; }
    }
}