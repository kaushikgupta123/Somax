using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common.Charts.Fusions
{
    public class multiSeriesLine2dModel
    {
        public multiSeriesLine2dModel()
        {
            chart = new multiSeriesLineChart();
            categories = new List<multiSeriesLineCategory>();
            dataset = new List<multiSeriesLineDataset>();
            trendlines = new List<Trendline>();
        }
        public multiSeriesLineChart chart { get; set; }
        public List<multiSeriesLineCategory> categories { get; set; }
        public List<multiSeriesLineDataset> dataset { get; set; }
        public List<Trendline> trendlines { get; set; }
    }
    public class multiSeriesLineChart
    {
        public multiSeriesLineChart()
        {
            plotFillAlpha = chartDefaults.PlotFillAlpha;
            divLineIsDashed = chartDefaults.Default1Value;
            divLineDashLen = chartDefaults.Default1Value;
            divLineGapLen = chartDefaults.Default1Value;
        }

        public string caption { get; set; }
        public string subCaption { get; set; }
        public string xAxisName { get; set; }
        public string plotFillAlpha { get; set; }
        public string divLineIsDashed { get; set; }
        public string divLineDashLen { get; set; }
        public string divLineGapLen { get; set; }
        public string theme { get => chartDefaults.DefaultTheme; }
        public string chartType { get => chartDefaults.MsColumn2dChartType; }
    }
    public class multiSeriesLineCategoryItems
    {
        public string label { get; set; }
        public string vline { get; set; }
        public string lineposition { get; set; }
        public string color { get; set; }
        public string labelHAlign { get; set; }
        public string labelPosition { get; set; }
        public string dashed { get; set; }
    }
    public class multiSeriesLineCategory
    {
        public multiSeriesLineCategory()
        {
            category = new List<multiSeriesLineCategoryItems>();
        }
        public List<multiSeriesLineCategoryItems> category { get; set; }
    }
    public class multiSeriesLineDatum
    {
        public string value { get; set; }
    }
    public class multiSeriesLineDataset
    {
        public multiSeriesLineDataset()
        {
            data = new List<multiSeriesLineDatum>();
        }
        public string seriesname { get; set; }
        public List<multiSeriesLineDatum> data { get; set; }
    }

    public class Trendline
    {
        public Trendline()
        {
            valueOnRight = chartDefaults.Default1Value;
            thickness = chartDefaults.Default1Value;
            showBelow = chartDefaults.Default1Value;
        }
        public string startvalue { get; set; }
        public string color { get; set; }
        public string valueOnRight { get; set; }
        public string displayvalue { get; set; }
        public string thickness { get; set; }
        public string showBelow { get; set; }
    }
}