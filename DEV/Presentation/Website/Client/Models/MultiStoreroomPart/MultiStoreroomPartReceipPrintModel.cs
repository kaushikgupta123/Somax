using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartReceipPrintModel
    {
        public string POClientLookupId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal UnitCost { get; set; }
    }
}