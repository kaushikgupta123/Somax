using System;

namespace Client.Models.Work_Order
{
    public class PartHistoryModel
    {
        public long PartHistoryId { get; set; }
        public long PartId { get; set; }
        public long PartStoreroomId { get; set; }
        public string PartClientLookupId { get; set; }
        public decimal TotalCost { get; set; }

        public long AccountId { get; set; }
        public decimal AverageCostBefore { get; set; }
        public decimal AverageCostAfter { get; set; }
        public string ChargeType_Primary { get; set; }
        public long ChargeToId_Primary { get; set; }
        public string Comments { get; set; }
        public decimal Cost { get; set; }
        public decimal CostAfter { get; set; }
        public decimal CostBefore { get; set; }
        public string Description { get; set; }
        public long DepartmentId { get; set; }
        public long PerformedById { get; set; }
        public decimal QtyAfter { get; set; }
        public decimal QtyBefore { get; set; }
        public long RequestorId { get; set; }
        public string StockType { get; set; }
        public long StoreroomId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal TransactionQuantity { get; set; }
        public string TransactionType { get; set; }
        public string UnitofMeasure { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string UPCCode { get; set; }
        //V2-610 parts grid

        public string PerformBy { get; set; }
        //V2-1034 parts grid
        public int TotalCount { get; set; }

    }
}