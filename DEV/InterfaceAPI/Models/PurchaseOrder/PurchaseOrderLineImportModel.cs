using System;

namespace InterfaceAPI.Models
{
    public class PurchaseOrderLineImportModel
    {
        public int EXPOId { get; set; }
        public int EXPOLineId { get; set; }
        public int SOMAXPRLineId { get; set; }
        public int LineNumber { get; set; }
        public string PartNumber { get; set; }
        public int PartId { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Status { get; set; }
        public string AccountCode { get; set; }
        public DateTime CreateDate { get; set; }
    }
}