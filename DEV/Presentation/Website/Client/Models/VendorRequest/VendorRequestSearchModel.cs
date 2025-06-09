using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.VendorRequest
{
    public class VendorRequestSearchModel
    {
        public long VendorRequestId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int TotalCount { get; set; }
    }
}