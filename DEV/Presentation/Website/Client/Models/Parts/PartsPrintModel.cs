namespace Client.Models.Parts
{
    public class PartsPrintModel
	{
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal OnHandQuantity { get; set; }
        public decimal MinimumQuantity { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerID { get; set; }
        public string StockType { get; set; }
        public string Section { get; set; }
        public string Row { get; set; }
        public string Bin { get; set; }
        public string UPCCode { get; set; }
        public string Shelf { get; set; }
        public string PreviousId { get; set; }
        public string PlaceArea { get; set; }
        public bool Consignment { get; set; }
        public decimal Maximum { get; set; }//V2-888
    }
}