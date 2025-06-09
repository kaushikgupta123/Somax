using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class ProjectLookupModel
    {
        public long ProjectID { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public long TotalCount { get; set; }
        
    }
}