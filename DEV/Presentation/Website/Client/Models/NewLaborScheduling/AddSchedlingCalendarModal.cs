using Client.CustomValidation;
using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.NewLaborScheduling
{
    public class AddSchedlingCalendarModal
    {
        [FutureDateValidation(ErrorMessage = "ValidateScheduleValueNotPastDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "ValidateScheduleDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime ScheduleDate { get; set; }
        
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnGlobalEstimateHrs|" + LocalizeResourceSetConstants.Global)]
        public decimal? Hours { get; set; }
        [Required(ErrorMessage = "ValidatePersonnel|" + LocalizeResourceSetConstants.Global)]
        [MinLength(1, ErrorMessage = "ValidatePersonnel|" + LocalizeResourceSetConstants.Global)]
        public string[] Personnels { get; set; }
        [Required(ErrorMessage = "ValidateWorkOrder|" + LocalizeResourceSetConstants.Global)]
        public long WorkOrderID { get; set; }
    }
}