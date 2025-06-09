namespace Client.Models.Sanitation
{
    public class ToolsDevExpressPrintModel
    {
        public long SanitationJobId { get; set; }
        public decimal? Quantity { get; set; }
        public string CategoryValue { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }

        #region Localization
        public string spnTools { get; set; }
        public string spnQuantity { get; set; }
        public string spnTool { get; set; }
        public string spnDescription { get; set; }
        public string spnInstructions { get; set; }
        #endregion Localization
    }
}
