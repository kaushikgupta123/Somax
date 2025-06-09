using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Project
{
    public class ProjectSearchModel
    {
        public long ProjectId { get; set; }
        public string ClientlookupId { get; set; }
        public string Description { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualFinish { get; set; }
        public string Status { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? CompleteDate { get; set; }
        public Int32 ChildCount { get; set; }
        public Int32 TotalCount { get; set; }
       

    }
}