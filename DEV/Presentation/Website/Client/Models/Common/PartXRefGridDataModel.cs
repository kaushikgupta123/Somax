namespace Client.Models.Common
{
    public class PartXRefGridDataModel
    {
        public long PartId { get; set; }
        public string ClientLookUpId { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerID { get; set; }
        public string StockType { get; set; }
        public string IssueUnit { get; set; }//V2-1119
        public long TotalCount { get; set; }
        public string UPCcode { get; set; }
        public decimal AppliedCost { get; set; } //V2-1124
        public long PartStoreroomId { get; set; } // RKL-MAIL-Label Printing from Receipts
    }
}