using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.WorkOrderPlanning
{
    public class ResourceListPrintModel
    {
        public string PersonnelName { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public decimal ScheduledHours { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }

    }
}