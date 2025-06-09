using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string DefStoreroom { get; set; } //V2-1025
        public string StockType { get; set; }
        //public string Manufacturer { get; set; }
        //public string ManufacturerID { get; set; }
        public decimal? AppliedCost { get; set; }
        
    }
}