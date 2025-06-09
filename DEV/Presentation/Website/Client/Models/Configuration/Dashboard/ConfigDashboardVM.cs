using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Configuration.Dashboard
{
    public class ConfigDashboardVM : LocalisationBaseVM
    {
        public bool IsEnterPrise { get; set; }
        public bool IsSiteControl { get; set; }
        public bool IsSiteAdmin { get; set; }
        public bool IsSuperUser { get; set; }

        private string DefaultdateRangeValue = "0";
        public long LoggedInUserSiteId { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public string selectedDateRangeVal
        {
            get => DefaultdateRangeValue;
        }
        public IEnumerable<SelectListItem> DateRangeDropListForActivity { get; set; }
    }
}