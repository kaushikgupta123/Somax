using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common.Charts.Fusions
{
    public class scrollbar2dModel
    {
        public scrollbar2dModel()
        {
            chart = new scrollbar2dChart();
            categories = new List<scrollbar2dCategory>();
            dataset = new List<scrollbar2dDataset>();
        }
        public scrollbar2dChart chart { get; set; }
        public List<scrollbar2dCategory> categories { get; set; }
        public List<scrollbar2dDataset> dataset { get; set; }
    }

    public class scrollbar2dChart
    {
        public string theme { get => "fusion"; }
        public string caption { get; set; }
        public string subCaption { get; set; }
        public string plottooltext { get; set; }
        public string YAxisname { get; set; }
        public string XAxisname { get; set; }
    }

    public class scrollbar2dCategoryItem
    {
        public string label { get; set; }
    }

    public class scrollbar2dCategory
    {
        public scrollbar2dCategory()
        {
            category = new List<scrollbar2dCategoryItem>();
        }
        public List<scrollbar2dCategoryItem> category { get; set; }
    }

    public class scrollbar2dDatum
    {
        public string value { get; set; }
    }

    public class scrollbar2dDataset
    {
        public scrollbar2dDataset()
        {
            data = new List<scrollbar2dDatum>();
        }
        public List<scrollbar2dDatum> data { get; set; }
    }

}