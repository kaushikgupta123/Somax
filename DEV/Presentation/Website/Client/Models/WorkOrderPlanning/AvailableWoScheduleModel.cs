using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.WorkOrderPlanning
{
    public class AvailableWoScheduleModel
    {
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string ChargeTo { get; set; }
        public string ChargeToName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string DownRequired { get; set; }
        public string Assigned { get; set; }

        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public Decimal? Duration { get; set; }
        public DateTime? RequiredDate { get; set; }
        public int TotalCount { get; set; }
    }
}