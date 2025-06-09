using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.WorkOrderPlanning
{
    public class WorkOrderPlanningPrintModel
    {
        public long PlanID { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Completed { get; set; }
        
    }
}