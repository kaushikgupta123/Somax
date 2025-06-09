using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.PreventiveMaintenance
{
    public class ScheduleAssignModel
    {
        public long PersonnelId { get; set; }
        public long PMSchedAssignId { get; set; }
        public long PrevMaintSchedId { get; set; }
        [Required(ErrorMessage = "ValidatePersonnel|" + LocalizeResourceSetConstants.Global)]
        public string Assignment_PClientLookupId { get; set; }
        public string PersonnelFullName { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(double.Epsilon, 99999999.99, ErrorMessage = "globalGreaterThan0TwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Assignment_ScheduledHours { get; set; }
    }
}