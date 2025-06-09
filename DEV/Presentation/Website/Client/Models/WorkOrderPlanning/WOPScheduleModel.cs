using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Client.Models.WorkOrderPlanning
{
    public class WOPScheduleModel
    {
        public long PersonnelId { get; set; }
        public IEnumerable<SelectListItem> Personnellist { get; set; }
        [Required(ErrorMessage = "ValidateScheduleDate|" + LocalizeResourceSetConstants.Global)]
        [DateOnlybetweenStartDateAndEndDate(otherPropertyName1 = "WOPStartDate", otherPropertyName2 = "WOPEndDate", ErrorMessage = "ValidateDateOnlyBetweenSelectedWOPStartAndEndDateErrMsg|" + LocalizeResourceSetConstants.WorkOrderPlanning)]
        public DateTime? Schedulestartdate { get; set; }
        public decimal ScheduledDuration { get; set; }
        public long WorkOrderPlanId { get; set; }
        [Required(ErrorMessage = "ValidateAssignedUser|" + LocalizeResourceSetConstants.Global)]
        public List<string> PersonnelIds { get; set; }
        public string WorkOrderIds { get; set; }
        public long AssignedPersonnelId { get; set; }
        public IEnumerable<SelectListItem> Assignedlist { get; set; }
        public string ClientLookupIds { get; set; }
        public string Status { get; set; }
        public DateTime? WOPStartDate { get; set; }
        public DateTime? WOPEndDate { get; set; }
    }
}