using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Project
{
    public class ProjectTaskModel
    {
        public long ProjectTaskId { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public string WorkOrderDescription { get; set; }
        public string StartDate { get; set; }
        public string Enddate { get; set; }
        public decimal Progress { get; set; }
        public int ProgressPercentage { get; set; }
    }
}