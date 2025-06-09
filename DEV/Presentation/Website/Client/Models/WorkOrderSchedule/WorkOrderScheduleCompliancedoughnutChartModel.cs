using Client.Models.Common.Charts;

using System.Collections.Generic;

namespace Client.Models.WorkOrderSchedule
{
    public class WorkOrderScheduleCompliancedoughnutChartModel
    {
        public WorkOrderScheduleCompliancedoughnutChartModel()
        {
            data = new List<doughnut2dChartData>();
            info = new doughnut2dChartInfo();
        }
        public doughnut2dChartInfo info { get; set; }
        public List<doughnut2dChartData> data { get; set; }
    }
}