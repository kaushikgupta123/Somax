using System;

namespace Client.Models.Parts
{
  
    public class PartSummaryModel
    {
        public string PartImageUrl { get; set; }
        public string ClientLookupId { get; set; }
        public decimal? OnHandQuantity { get; set; }
        public decimal? OnOrderQuantity { get; set; }
        public decimal? OnRequestQuantity { get; set; }
        public string Description { get; set; }

        public bool ClientOnPremise { get; set; }
        public bool InactiveFlag { get; set; }
        #region V2-836
        public decimal? MinimumQuantity { get; set; }
        public decimal? Maximum { get; set; }
        public decimal? AppliedCost { get; set; }
        public string IssueUnit { get; set; }
        public string StockType { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerID { get; set; }
        public string PlaceArea { get; set; }
        public string Section { get; set; }
        public string Shelf { get; set; }
        public string Row { get; set; }
        public string Bin { get; set; }
        #endregion
    }
}