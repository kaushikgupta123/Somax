using Common.Constants;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Parts
{
    public class POPRModel
    {
        public long PartId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public string Vendor { get; set; }
        public string VendorName { get; set; }      
        public DateTime CreateDate { get; set; }
        public string POType { get; set; }
        public long PoPrId { get; set; }
    }
}