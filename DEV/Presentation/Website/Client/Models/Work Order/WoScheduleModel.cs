using Client.CustomValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Common.Constants;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{

    public class WoScheduleModel
    {
        public long PersonnelId { get; set; }
        public IEnumerable<SelectListItem> Personnellist { get; set; }
        [FutureDateValidation(ErrorMessage = "Schedule  value should not be a past date")]
        [Required(ErrorMessage = "WOSelectScheduledDateErrorMessage|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public DateTime? Schedulestartdate { get; set; }
        public decimal ScheduledDuration { get; set; }
        public long WorkOrderId { get; set; }
        [Required(ErrorMessage = "WOSelectAssignedErrorMessage|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public List<string> PersonnelIds { get; set; }
        public string WorkOrderIds { get; set; }
        public long AssignedPersonnelId { get; set; }
        public IEnumerable<SelectListItem> Assignedlist { get; set; }
        public string ClientLookupIds { get; set; }
        public string Status { get; set; }
    }
}