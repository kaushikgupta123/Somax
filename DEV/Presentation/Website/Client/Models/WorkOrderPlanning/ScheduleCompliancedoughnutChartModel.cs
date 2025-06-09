using Client.Models.Common.Charts;

using System.Collections.Generic;

namespace Client.Models.WorkOrderPlanning
{
    public class ScheduleCompliancedoughnutChartModel
    {
        public ScheduleCompliancedoughnutChartModel()
        {
            data = new List<doughnut2dChartData>();
            info = new doughnut2dChartInfo();
        }
        public doughnut2dChartInfo info { get; set; }
        public List<doughnut2dChartData> data { get; set; }
    }
}