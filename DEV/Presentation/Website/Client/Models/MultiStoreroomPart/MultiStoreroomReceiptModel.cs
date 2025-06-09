using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomReceiptModel
    {
        public long PartID { get; set; }
        public string POClientLookupId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public string receiptdtselector { get; set; }
        public IEnumerable<SelectListItem> ReceiptDateList { get; set; }
    }
}