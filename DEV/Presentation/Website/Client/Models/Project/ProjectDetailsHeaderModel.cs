using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Project
{
    public class ProjectDetailsHeaderModel
    {
        public long ProjectId { get; set; }
        public string ClientlookupId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Coordinator { get; set; }
        public string ScheduleStart { get; set; }
        public string ScheduleFinish { get; set; }
        public string CompleteDate { get; set; }
        public decimal Budget { get; set; }
    }
}