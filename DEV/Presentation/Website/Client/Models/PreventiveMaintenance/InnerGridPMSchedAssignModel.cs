using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PreventiveMaintenance
{
    public class InnerGridPMSchedAssignModel
    {
        public long PMSchedAssignId { get; set; }
        public long PersonnelId { get; set; }
        public decimal ScheduledHours { get; set; }
        public string ClientLookupId { get; set; }
        public string PersonnelFullName { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public int offset1 { get; set; }
        public int nextrow { get; set; }
        public int TotalCount { get; set; }
    }
}