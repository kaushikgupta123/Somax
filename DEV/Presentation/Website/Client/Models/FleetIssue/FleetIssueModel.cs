using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.FleetIssue
{
    public class FleetIssueModel
    {
        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "SelectEquipmentIDErrorMessage|" + LocalizeResourceSetConstants.Global)]
        [StringLength(31, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "EquipmentIDRegErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string ClientLookupId { get; set; }
        public string EquipmentID { get; set; }

        [Display(Name = "spnRecordDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "DateDownErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string RecordDate { get; set; }
        [Required(ErrorMessage = "TimeDownErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]) (am|pm|AM|PM)$", ErrorMessage = "globalTimeErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string RecordTime { get; set; }

        public long FleetIssuesId { get; set; }
        [Display(Name = "spnDriverName|" + LocalizeResourceSetConstants.Global)]
        public string DriverName { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public string Defects { get; set; }
        [Display(Name = "spnDefects|" + LocalizeResourceSetConstants.Global)]
        public List<string> DefectsIds { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForFFRecorddate { get; set; }
        [DefaultValue("add")]
        public string Pagetype { get; set; }

        public IEnumerable<SelectListItem> IssueViewList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListAllStatus { get; set; }
    }
}