using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.ShipToAddress
{
    public class ShipToPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Address1 { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
    }
}