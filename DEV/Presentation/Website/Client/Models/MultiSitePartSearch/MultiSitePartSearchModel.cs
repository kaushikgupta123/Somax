using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.MultiSitePartSearch
{
    public class MultiSitePartSearchModel
    {
        public long ClientId { get; set; }
        public long PartId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }      
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int UpdateIndex { get; set; }
     
    }
}