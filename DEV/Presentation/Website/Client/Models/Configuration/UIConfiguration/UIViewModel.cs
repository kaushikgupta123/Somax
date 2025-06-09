using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Configuration.UIConfiguration
{
    public class UIViewModel
    {
        public UIViewModel()
        {
            ViewNameLookUpList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> ViewNameLookUpList { get; set; }
        public UiViewDetails uiViewDetails { get; set; }

    }

    public class UIViewNameModel
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
    public class UiViewDetails
    {
        public long UiViewId { get; set; }
        public string ViewName { get; set; }
        public string Description { get; set; }
        public string ViewType { get; set; }
    }
}