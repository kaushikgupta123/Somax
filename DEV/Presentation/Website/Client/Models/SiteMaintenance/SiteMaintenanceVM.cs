using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models
{
    public class SiteMaintenanceVM : LocalisationBaseVM
    {
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
    }
    public class SiteMaintenanceModel
    {
        public long CallerUserInfoId { get; set; }
        public string CallerUserName { get; set; }
        public long SiteMaintenanceId { get; set; }
        public string HeaderText { get; set; }
        public string MessageText { get; set; }
        public DateTime DowntimeStart { get; set; }
        public DateTime DowntimeEnd { get; set; }
        public string LoginPageMessage { get; set; }
        public string DashboardMessage { get; set; }
        public long UpdateIndex { get; set; }
        public string EasternStartTime { get; set; }
        public string EasternEndTime { get; set; }
        public string MaintenenceMessage { get; set; }
    }
}