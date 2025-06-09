using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PreventiveMaintenance
{
    public class PreventiveMaintenanceForcastPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public DateTime? SchedueledDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string Assigned { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ChargeToName { get; set; }
        public decimal Duration { get; set; }
        public decimal EstLaborHours { get; set; }
        public decimal EstLaborCost { get; set; }    
        public decimal EstOtherCost { get; set; }
        public decimal EstMaterialCost { get; set; }
        public bool? DownRequired { get; set; }
        public string Shift { get; set; }
        public string Type { get; set; }

        
    }
}