using Client.CustomValidation;
using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.WorkOrderPlanning
{
    public class ResourceCalendarEditScheduleModel
    {
        [Required(ErrorMessage = "ValidateScheduleDate|" + LocalizeResourceSetConstants.Global)]
        [DateOnlybetweenStartDateAndEndDate(otherPropertyName1 = "WorkOrderPlanStartDate", otherPropertyName2 = "WorkOrderPlanEndDate", ErrorMessage = "Scheduled date can only be between the selected work order plan start date and end date")]
        public DateTime ScheduleDate { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnGlobalEstimateHrs|" + LocalizeResourceSetConstants.Global)]
        public decimal? Hours { get; set; }
        public string PersonnelName { get; set; }
        public long WorkOrderID { get; set; }
        public long WorkOrderScheduledID { get; set; }
        public string Description { get; set; }
        public string ClientLookupId { get; set; }
        public DateTime WorkOrderPlanStartDate { get; set; }
        public DateTime WorkOrderPlanEndDate { get; set; }
    }
}