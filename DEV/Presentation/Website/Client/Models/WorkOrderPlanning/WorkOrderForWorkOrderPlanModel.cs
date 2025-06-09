using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.WorkOrderPlanning
{
    public class WorkOrderForWorkOrderPlanModel
    {
        public long WOPlanLineItemId { get; set; }
        public string WOPlanLineItemType { get; set; }
        public long WorkOrderId { get; set; }
        public long WorkOrderPlanId { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public int TotalCount { get; set; }
    }
}