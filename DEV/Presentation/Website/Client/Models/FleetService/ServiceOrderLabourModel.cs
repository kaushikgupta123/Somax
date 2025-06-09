using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.FleetService
{
    public class ServiceOrderLabourModel
    {
        [Display(Name = "spnPersonnel|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validpersonnel|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public long? PersonnelID { get; set; }
        [Display(Name = "spnDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "spnHours|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "vaildHours|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(double.Epsilon, 99999999.99, ErrorMessage = "globalGreaterThan0TwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Hours { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public long ServiceOrderLineItemId { get; set; }
        public long ServiceOrderId { get; set; }
        public long TimecardId { get; set; }
        [Display(Name = "spnWorkAccomplishedCode|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        [Required(ErrorMessage = "WorkAccomplishedCodeRequired|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public string VMRSWorkAccomplished { get; set; }
        public string VMRSWorkAccomplishedCode { get; set; }
    }
}