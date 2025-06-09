using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartLookup
{
    public class LineItemOnOrderCheckModel
    {
        public string ClientLookupId { get; set; }
        public int LineNumber { get; set; }
        public string Name { get; set; }
        public decimal? OrderQuantity { get; set; }
        public string UnitofMeasure { get; set; }
        public DateTime? CreateDate { get; set; }
        public long TotalCount { get; set; }

    }
}