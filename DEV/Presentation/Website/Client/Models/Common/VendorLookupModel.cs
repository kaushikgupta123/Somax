using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class VendorLookupModel
    {
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long TotalCount { get; set; }
        public long VendorID { get; set; }
        
        //V2-759
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
    }
}