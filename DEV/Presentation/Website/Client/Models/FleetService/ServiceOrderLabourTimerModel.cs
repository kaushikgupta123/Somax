using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.FleetService
{
    public class ServiceOrderLabourTimerModel
    {    
        public DateTime? StartDate { get; set; }        
        public long ServiceOrderLineItemId { get; set; }
        public long ServiceOrderId { get; set; }
        [Display(Name = "spnWorkAccomplishedCode|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        [Required(ErrorMessage = "WorkAccomplishedCodeRequired|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public string VMRSWorkAccomplished { get; set; }
        public TimeSpan TimeSpan { get; set; }
    }
}