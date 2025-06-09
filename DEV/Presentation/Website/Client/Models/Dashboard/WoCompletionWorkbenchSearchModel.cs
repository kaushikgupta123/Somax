using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class WoCompletionWorkbenchSearchModel
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string AssetName { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Priority { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public int TotalCount { get; set; }

    }
}