using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Project
{
    public class WorkOrderForProjectDetailsLookupListModel
    {
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string ChargeTo { get; set; }
        public string ChargeTo_Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public string RequiredDate { get; set; }
        public int TotalCount { get; set; }
       
    }
}