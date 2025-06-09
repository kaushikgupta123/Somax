using System.Collections.Generic;

namespace Client.Models
{
    public class WorkOrderSourceChartData
    {
        public WorkOrderSourceChartData()
        {
            data = new List<long>();
            SourceType = new List<string>();
        }
        public string label { get; set; }

        public string backgroundColor { get; set; }
        public List<long> data { get; set; }
        public List<string> SourceType { get; set; }
        public bool fill { get; set; }
        public string borderColor { get; set; }
        public int borderWidth { get; set; }
        public string pointBorderColor { get; set; }
        public int pointRadius { get; set; }
        public int pointHoverRadius { get; set; }
        public int pointHitRadius { get; set; }
        public int pointBorderWidth { get; set; }
    }
}