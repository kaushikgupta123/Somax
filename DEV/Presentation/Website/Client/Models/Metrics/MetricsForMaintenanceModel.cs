using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class MetricsForMaintenanceModel
    {
        public string SiteName { get; set; }
        public decimal WorkOrdersCreated { get; set; }
        public decimal WorkOrdersCompleted { get; set; }
        public decimal LaborHours { get; set; }
    }
}