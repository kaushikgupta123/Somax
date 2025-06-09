using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Parts
{
    public class PartGridPhysicalInvList
    {
        public long? PartId { get; set; }
        public string PartClientLookupId { get; set; }
        public string PartUPCCode { get; set; }
        public string Description { get; set; }
        public decimal? QtyOnHand { get; set; }
        public decimal? QuantityCount { get; set; }
        public int PartStoreroomUpdateIndex { get; set; }
        public string Section { get; set; } //Location1_1
        public string Row { get; set; } //Location1_2
        public string Shelf { get; set; } //Location1_3
        public string Bin { get; set; } //Location1_4
        public string ErrorMessages { get; set; } //Location1_4
        public long? PerformedById { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? PartAverageCost { get; set; }
    }
}