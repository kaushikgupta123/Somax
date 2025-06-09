namespace Client.Models.Work_Order
{
    public class OthersDevExpressPrintModel
    {
        public string Source { get; set; }
        public string Description { get; set; }
        public long VendorId { get; set; }
        public string VendorClientLookupId { get; set; }       
        public decimal? UnitCost { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TotalCost { get; set; }
        #region Localization
        public string spnOthers { get; set; }
        public string spnSource { get; set; }
        public string spnVendor { get; set; }
        public string spnDescription { get; set; }
        public string spnTotalCost { get; set; }
        public string spnUnitCost { get; set; }
        public string spnQuantity { get; set; }
        #endregion Localization
    }
}