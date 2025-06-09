using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.WorkOrderPlanning
{
    public class RemoveScheduleListModel
    {
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        public long WorkOrderSchedId { get; set; }
    }
    public class RemoveScheduleModel
    {
        public List<RemoveScheduleListModel> list { get; set; }

    }
}