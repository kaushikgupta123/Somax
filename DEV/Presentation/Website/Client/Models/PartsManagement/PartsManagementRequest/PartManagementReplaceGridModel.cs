using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class PartManagementReplaceGridModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public long TotalCount { get; set; }
    }
}