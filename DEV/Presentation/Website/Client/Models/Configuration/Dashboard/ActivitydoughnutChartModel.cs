using Client.Models.Common.Charts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.Dashboard
{
    public class ActivitydoughnutChartModel
    {
        public ActivitydoughnutChartModel()
        {
            data = new List<doughnut2dChartData>();
            info = new doughnut2dChartInfo();
        }
        public doughnut2dChartInfo info { get; set; }
        public List<doughnut2dChartData> data { get; set; }
    }
}