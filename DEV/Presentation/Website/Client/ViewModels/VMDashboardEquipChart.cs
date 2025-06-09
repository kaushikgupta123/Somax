using System.Collections.Generic;

namespace Client.ViewModels
{
    public class VMDashboardEquipChart
    {
        public string EQNAME { get; set; }
        public long MINS { get; set; }

        public List<VMDashboardEquipChart> VMDashboardEquipCharts { get; set; }
    }
}