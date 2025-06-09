using Client.CustomValidation;
using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.NewLaborScheduling
{
    public class EditSchedlingCalendarModal
    {
        [FutureDateValidation(ErrorMessage = "ValidateScheduleValueNotPastDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "ValidateScheduleDate|" + LocalizeResourceSetConstants.Global)]
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
    }
}