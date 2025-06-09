using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartCycleCount
{
    public class PartCycleCountPrintModel
    {
        public string PartId { get; set; }
        public string PartDescription { get; set; }
        public decimal? QtyOnHand { get; set; }
        public int Count { get; set; }
        public string Area { get; set; }
        public string Section { get; set; }
        public string Row { get; set; }
        public string Shelf { get; set; }
        public string Bin { get; set; }
        public decimal ? Variance { get; set; }
    }
}