using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.Site
{
    public class SiteSearchModel
    {
        public long SiteId { get; set; }
        public string Name { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }       
        public int TotalCount { get; set; }
    }
}