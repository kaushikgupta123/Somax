namespace Client.Models.Work_Order
{
    public class SummaryDevExpressPrintModel
    {
        public decimal? PartsCosts { get; set; }
        public decimal? CraftsCosts { get; set; }
        public decimal? OtherExternalCosts { get; set; }
        public decimal? OtherInternalCosts { get; set; }
        public decimal? TotalCost { get; set; }
        #region Localization
        public string spnSummary { get; set; }
        public string spnPartsCosts { get; set; }
        public string spnCraftCosts { get; set; }
        public string spnOtherExternalCosts { get; set; }
        public string spnOtherInternalCosts { get; set; }
        public string spnTotalCost { get; set; }
        #endregion Localization
    }
}