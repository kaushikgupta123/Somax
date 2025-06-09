using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Dashboard
{
    public class DashboardWROnlyVM : DashboardVM
    {
        public IEnumerable<SelectListItem> ScheduleWorkList { get; set; }
        public IEnumerable<SelectListItem> SanitationList { get; set; }
    }

}