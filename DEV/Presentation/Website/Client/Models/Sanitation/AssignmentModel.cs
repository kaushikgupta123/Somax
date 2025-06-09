using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Sanitation
{
    public class AssignmentModel
    {
        public long SanitationJobScheduleId { get; set; }

        public long? SiteId { get; set; }

        [Required(ErrorMessage = "spnReqPersonnelIDrequired|" + LocalizeResourceSetConstants.SanitationDetails)]
        public long PersonnelId { get; set; }
        [Display(Name = "spnScheduledStartDate|" + LocalizeResourceSetConstants.SanitationDetails)]
        [Required(ErrorMessage = "spnReqScheduledStartDate|" + LocalizeResourceSetConstants.SanitationDetails)]
        public DateTime? ScheduledStartDate { get; set; }
     
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(double.Epsilon, 99999999.99, ErrorMessage = "globalGreaterThan0TwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnReqScheduleHours|" + LocalizeResourceSetConstants.SanitationDetails)]

        public decimal? ScheduledHours { get; set; }

        public int UpdateIndex { get; set; }

        public string ChargeType_Primary { get; set; }

        public long SanitationJobId { get; set; }

        public IEnumerable<SelectListItem> PersonnelIdList { get; set; }

        public decimal? Value { get; set; }

        public string Name { get; set; }

        public string ClientLookupId { get; set; }
    }
}