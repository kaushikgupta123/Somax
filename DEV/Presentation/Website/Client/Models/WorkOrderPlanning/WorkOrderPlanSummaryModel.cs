using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.WorkOrderPlanning
{
    public class WorkOrderPlanSummaryModel
    {
        public long WorkOrderPlanId { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CompleteDate { get; set; }
        public string Status { get; set; }
        public bool LockPlan { get; set; }
        public string PlannerName { get; set; }
    }
}