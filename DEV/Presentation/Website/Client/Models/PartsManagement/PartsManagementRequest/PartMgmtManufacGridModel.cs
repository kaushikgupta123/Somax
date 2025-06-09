using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class PartMgmtManufacGridModel
    {
        public long? ManufacturerMasterId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long TotalCount { get; set; }
    }
}