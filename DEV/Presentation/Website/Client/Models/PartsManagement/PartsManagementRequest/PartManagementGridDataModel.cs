using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class PartManagementGridDataModel
    {
        public long? PartMasterId { get; set; }
        public string ClientLookupId { get; set; }
        public string LongDescription { get; set; }
        public long TotalCount { get; set; }
    }
}