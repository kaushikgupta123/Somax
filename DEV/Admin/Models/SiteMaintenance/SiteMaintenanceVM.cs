using Admin.Models.Client;

using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Admin.Models
{
    public class SiteMaintenanceVM : LocalisationBaseVM
    {
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }

        //V2-994
        public SiteMaintenanceModel SiteMaintenanceModel { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]

        [Display(Name = "spnDowntimeStart|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "vaildDate|" + LocalizeResourceSetConstants.Global)]
        public string DowntimeStartDate { get; set; }
        [Required(ErrorMessage = "vaildTime|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]) (am|pm|AM|PM)$", ErrorMessage = "globalTimeErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string DowntimeStartTime { get; set; }
        [Display(Name = "spnDowntimeEnd|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "vaildDate|" + LocalizeResourceSetConstants.Global)]
        public string DowntimeEndDate { get; set; }
        [Required(ErrorMessage = "vaildTime|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]) (am|pm|AM|PM)$", ErrorMessage = "globalTimeErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string DowntimeEndTime { get; set; }
        public DateTime CreateDate { get; set; }
        [Display(Name = "spnLoginPageMessage|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnLoginPageMessageErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string LoginPageMessage { get; set; }
        [Display(Name = "spnDashboardPageMessage|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnDashboardPageMessageErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string DashboardMessage { get; set; }
        public long UpdateIndex { get; set; }
        public string EasternStartTime { get; set; }
        public string EasternEndTime { get; set; }
        public int TotalCount { get; set; }

    }
}