namespace Client.Models.Work_Order
{
    public class PartDevExpressPrintModel
    {
        public string PartClientlookupId { get; set; }
        public string Description { get; set; }
        public decimal? UnitCost{ get; set; }
        public string UOM { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TotalCost { get; set; }
        #region Localization
        public string spnPart { get; set; }
        public string spnParts { get; set; }
        public string spnUnitCost { get; set; }
        public string spnUOM { get; set; }
        public string spnQuantity { get; set; }
        public string spnTotalCost { get; set; }
        public string spnDescription { get; set; }
        #endregion Localization



    }
}