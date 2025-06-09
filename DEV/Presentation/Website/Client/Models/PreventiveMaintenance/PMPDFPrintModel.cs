using System.Collections.Generic;

namespace Client.Models.PreventiveMaintenance
{
    public class PMPDFPrintModel: PreventiveMaintenancePrintModel
    {
        public PMPDFPrintModel()
        {
            scheduleRecordsList = new List<ScheduleRecords>();
        }
        public List<ScheduleRecords> scheduleRecordsList { get; set; }

    }
}