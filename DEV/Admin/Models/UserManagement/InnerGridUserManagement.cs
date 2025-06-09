using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.UserManagement
{
    public class InnerGridUserManagement
    {
        public long SiteId { get; set; }
        public long CraftId { get; set; }
        public string Personnel_ClientLookupId { get; set; }
        public string SiteName { get; set; }
        public string CraftDescription { get; set; }
        public bool Buyer { get; set; }
        public bool Planner { get; set; }
    }
}