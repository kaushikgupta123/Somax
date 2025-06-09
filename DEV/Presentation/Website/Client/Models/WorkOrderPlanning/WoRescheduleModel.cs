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
    public class WoRescheduleModel
    {
        public long PersonnelId { get; set; }
       
        [DateOnlybetweenStartDateAndEndDateAttribute(otherPropertyName1 = "WorkOrderPlanStartDate", otherPropertyName2 = "WorkOrderPlanEndDate", ErrorMessage = "ValidateDateOnlyBetweenSelectedWOPStartAndEndDateErrMsg|" + LocalizeResourceSetConstants.WorkOrderPlanning)]
        [Required(ErrorMessage = "ValidateScheduleDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? Schedulestartdate { get; set; }
        public decimal ScheduledDuration { get; set; }
        public long WorkOrderId { get; set; }
        [Required(ErrorMessage = "ValidateAssignedUser|" + LocalizeResourceSetConstants.Global)]
        public List<string> PersonnelIds { get; set; }
        public string WorkOrderIds { get; set; }
        public long AssignedPersonnelId { get; set; }
        public IEnumerable<SelectListItem> Assignedlist { get; set; }
        public string ScheduledDurations { get; set; }
        public string ClientLookupIds { get; set; }
        public string Status { get; set; }
        public DateTime? WorkOrderPlanStartDate { get; set; }

        public DateTime? WorkOrderPlanEndDate { get; set; }
    }
}