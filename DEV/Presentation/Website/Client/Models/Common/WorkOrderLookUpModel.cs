using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common
{
    public class WorkOrderLookUpModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string ChargeTo { get; set; }
        public string WorkAssigned { get; set; }
        public string Requestor { get; set; }
        public string Status { get; set; }
        public long TotalCount { get; set; }
        public long? WorkOrderId { get; set; }
    }
}