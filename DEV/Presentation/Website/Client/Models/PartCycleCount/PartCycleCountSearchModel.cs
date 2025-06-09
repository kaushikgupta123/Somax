using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartCycleCount
{
    public class PartCycleCountSearchModel
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public long PartId { get; set; }
        public string ClientLookupId { get; set; }
        public string PartDescription { get; set; }
        public decimal? QtyOnHand { get; set; }
        public decimal? QuantityCount { get; set; }   
        public decimal? Variance { get; set; }
        public string Section { get; set; } //Location1_1
        public string Row { get; set; } //Location1_2
        public string Shelf { get; set; } //Location1_3
        public string Bin { get; set; } //Location1_4
        public string Area { get; set; } //Location1_5

        public int Count { get; set; }
        public int TotalCount { get; set; }
        public long PartHistoryId { get; set; }
        public long RowNum { get; set; }
    }
}