using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.Site
{
    public class SiteSummaryModel
    {
        public long SiteId { get; set; }
        public long ClientId { get; set; }
        public string Name { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
      
    }
}