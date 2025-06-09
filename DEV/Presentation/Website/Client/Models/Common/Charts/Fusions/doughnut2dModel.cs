namespace Client.Models.Common.Charts
{
    public class doughnut2dChartInfo
    {
        public bool showLegend { get; set; }
        public string caption { get; set; }
        public string subCaption { get; set; }
        public string showpercentvalues { get; set; }
        public string numberPrefix { get; set; }
        public string numberSuffix { get; set; }
        public string defaultCenterLabel { get; set; }
        public string centerLabel { get; set; }
        public string decimals { get; set; }
        public string theme { get => "fusion"; }
    }
    public class doughnut2dChartData
    {
        public string label { get; set; }
        public string value { get; set; }
    }
}