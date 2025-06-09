using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Dashboard
{
    public class DashboardWoTaskModel
    {
        public long WorkOrderTaskId { get; set; }
        public string ClientLookupId { get; set; }
        public long WorkOrderId { get; set; }
        public string Description { get; set; }
        public string TaskNumber { get; set; }
        public string ChargeToClientLookupId { get; set; }

        public string Status { get; set; }
        public int updatedindex { get; set; }

        public int TotalCount { get; set; }
        public string CancelReason { get; set; }
        public IEnumerable<SelectListItem> CancelReasonList { get; set; }
    }
}