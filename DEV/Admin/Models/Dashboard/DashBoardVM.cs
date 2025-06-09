using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models.Dashboard
{
    public class DashBoardVM : LocalisationBaseVM
    {
        public string DashboardGridHours { get; set; }
        public IEnumerable<SelectListItem> DashboardGridHoursList { get; set; }
    }
}