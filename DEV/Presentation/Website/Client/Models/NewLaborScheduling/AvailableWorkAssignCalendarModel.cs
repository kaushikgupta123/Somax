using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.NewLaborScheduling
{
    public class AvailableWorkAssignCalendarModel
    {
        public long PersonnelId { get; set; }
        public IEnumerable<SelectListItem> Personnellist { get; set; }
        [FutureDateValidation(ErrorMessage = "ValidateScheduleValueNotPastDate|" + LocalizeResourceSetConstants.Global )]
        [Required(ErrorMessage = "ValidateScheduleDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? Schedulestartdate { get; set; }
        [Required(ErrorMessage = "ValdateEstimatedHours|" + LocalizeResourceSetConstants.Global)]        
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnGlobalEstimateHrs|" + LocalizeResourceSetConstants.Global)]
        public decimal ScheduledDuration { get; set; }
        public long WorkOrderId { get; set; }
        [Required(ErrorMessage = "ValidateAssignedUser|" + LocalizeResourceSetConstants.Global  )]
        [MinLength(1, ErrorMessage = "ValidateAssignedUser|" + LocalizeResourceSetConstants.Global )]
        //public List<string> PersonnelIds { get; set; }
        public string[] PersonnelIds { get; set; }
        public string WorkOrderIds { get; set; }
        public long AssignedPersonnelId { get; set; }
        public IEnumerable<SelectListItem> Assignedlist { get; set; }
        public string ClientLookupIds { get; set; }
        public string Status { get; set; }
    }
}