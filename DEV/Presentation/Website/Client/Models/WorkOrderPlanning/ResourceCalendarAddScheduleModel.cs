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
    public class ResourceCalendarAddScheduleModel
    {
        [DateOnlybetweenStartDateAndEndDateAttribute(otherPropertyName1 = "WorkOrderPlanStartDate", otherPropertyName2 = "WorkOrderPlanEndDate", ErrorMessage = "ValidateDateOnlyBetweenSelectedWOPStartAndEndDateErrMsg|" + LocalizeResourceSetConstants.WorkOrderPlanning)]
        [Required(ErrorMessage = "ValidateScheduleDate|" + LocalizeResourceSetConstants.Global)]      
        public DateTime? Schedulestartdate { get; set; }

        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnGlobalEstimateHrs|" + LocalizeResourceSetConstants.Global)]
        public decimal? ScheduledDuration { get; set; }

        [Required(ErrorMessage = "ValidateWorkOrder|" + LocalizeResourceSetConstants.Global)]
        public long WorkOrderId { get; set; }

        [Required(ErrorMessage = "ValidatePersonnel|" + LocalizeResourceSetConstants.Global)]
        [MinLength(1, ErrorMessage = "ValidatePersonnel|" + LocalizeResourceSetConstants.Global)]
        public string[] PersonnelIds { get; set; }

        public DateTime? WorkOrderPlanStartDate { get; set; }
        public DateTime? WorkOrderPlanEndDate { get; set; }

    }
}