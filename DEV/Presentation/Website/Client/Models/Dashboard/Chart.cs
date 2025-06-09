using System;
using System.Collections.Generic;

namespace Client.Models
{
    public class ChartFusion
    {
        public string label { get; set; }
        public long value { get; set; }
         public string colors { get; set; }

    }

    public class MultiSeriesChart
    {
       public string seriesname { get; set; }
       public List<SeriesdataValue> data { get; set; }
    }

    public class ScheduleChart
    {
        public string Percent { get; set; }
        public List<WorkOrderPlannedStatus> chartdata = new List<WorkOrderPlannedStatus>();
    }

    public class ProjectTaskScheduleChart
    {
        public string Percent { get; set; }
        public List<ProjectTaskStatus> chartdata = new List<ProjectTaskStatus>();
    }

    public class OverLappingname
    {
       public string label { get; set; }
    }
    public class OverLappingChart
    {
        public List<MultiSeriesChart> multiseries { get;set;}
        public List<OverLappingname> category { get; set; }
    }

    public class SeriesdataValue
    {
        public string value { get; set; }
    }


    public class WorkOrderPlannedStatus
    {
        public string label { get; set; }
        public decimal value { get; set; }
    }

    public class ProjectTaskStatus
    {
        public string label { get; set; }
        public decimal value { get; set; }
    }

    public class ChartFusionLabour
    {
        public string label { get; set; }
        public decimal value { get; set; }
        public string colors { get; set; }
 
    }
    public class Chart
    {
        public string[] labels { get; set; }
        public List<Datasets> datasets { get; set; }
    }
    public class Datasets
    {
        public string label { get; set; }
        public string[] backgroundColor { get; set; }
        public string[] borderColor { get; set; }
        public string borderWidth { get; set; }
        public long[] data { get; set; }
    }
    public class Metrics
    {
        public string MetricName { get; set; }
        public long MetricValue { get; set; }
        public DateTime DataDate { get; set; }

    }
    public class WoLaborHr
    {
        public string PID { get; set; }
        public decimal Hrs { get; set; }
    }
    public class WorkOrderBacklogDb
    {
        public string WoCount { get; set; }
        public string DateRange { get; set; }
    }
    #region Fleet Only
    public class SoLaborHr
    {
        public string PID { get; set; }
        public decimal Hrs { get; set; }
    }
    public class ServiceOrderBacklogDb
    {
        public string SoCount { get; set; }
        public string DateRange { get; set; }
    }
    #endregion

    #region
    public class EnterpriseUserBarChartBySite
    {
        public string Site { get; set; }
        public decimal Value { get; set; }
    }
    #endregion
}