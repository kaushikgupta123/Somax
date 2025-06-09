using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.WorkOrderPlanning
{
    public class WorkOrderPlanPDFPrintModel:WorkOrderPlanningPrintModel
    {
        public WorkOrderPlanPDFPrintModel()
        {
            WOPlanLineItemModelList = new List<WOPlanLineItemModel>();
        }
        public List<WOPlanLineItemModel> WOPlanLineItemModelList { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public string CreatedDateString { get; set; }
        public string CompletedDateString { get; set; }
    }
}