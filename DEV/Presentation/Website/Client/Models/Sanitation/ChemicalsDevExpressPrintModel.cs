namespace Client.Models.Sanitation
{
    public class ChemicalsDevExpressPrintModel
    {
        public long SanitationJobId { get; set; }
        public decimal? Quantity { get; set; }
        public string CategoryValue { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public string Dilution { get; set; }

        #region Localization
        public string spnChemicals { get; set; }
        public string spnQuantity { get; set; }
        public string spnChemical { get; set; }
        public string spnDescription { get; set; }
        public string spnInstructions { get; set; }
        public string spnDilution { get; set; }
        #endregion Localization
    }
}
