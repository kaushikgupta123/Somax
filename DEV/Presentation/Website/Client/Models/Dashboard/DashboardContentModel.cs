using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class DashboardContentModel
    {
        public long DashboardContentId { get; set; }
        public long DashboardListingId { get; set; }
        public long WidgetListingId { get; set; }
        public bool Display { get; set; }
        public bool Required { get; set; }
        public int GridColWidth { get; set; }
        public string ViewName { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string ClassList { get; set; }
        public string ViewPath { get; set; }
        public string JSPath { get; set; }
    }
}