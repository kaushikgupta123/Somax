using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common.Charts.Fusions
{
    public class overlapping2dModel
    {
        public overlapping2dModel()
        {
            chart = new overlappingChart();
            dataset = new List<overlappingChartDataset>();
            //categories = new List<overlappingChartCategory>();
            categories = new List<overlappingChartCategory>();
        }

        public overlappingChart chart { get; set; }
        public List<overlappingChartDataset> dataset { get; set; }
        //public List<overlappingChartCategory> categories { get; set; }
        public List<overlappingChartCategory> categories { get; set; }
    }

    public class overlappingChartDataset
    {
        public overlappingChartDataset()
        {
            data = new List<overlappingChartDatum>();
        }
        public string seriesname { get; set; }
        public List<overlappingChartDatum> data { get; set; }
    }

    public class overlappingChartDatum
    {
        public string value { get; set; }
    }

    public class overlappingChartCategoryItem
    {
        public string label { get; set; }
    }
    public class overlappingChartCategory
    {
        public overlappingChartCategory()
        {
            category = new List<overlappingChartCategoryItem>();
        }
        public List<overlappingChartCategoryItem> category { get; set; }

    }
    public class overlappingChart
    {
        public overlappingChart()
        {
            xAxisName = chartDefaults.xAxisName;
            showvalues = chartDefaults.showValues;
            drawcrossline = chartDefaults.Default1Value;
            plottooltext = chartDefaults.OverlappedChartPlotToolText;
        }

        public string xAxisName { get; set; }
        public string theme { get => chartDefaults.DefaultTheme; }
        public string showvalues { get; set; }
        public string drawcrossline { get; set; }
        public string plottooltext { get; set; }
        public string chartType { get => chartDefaults.OverlappedChartType; }
    }
}