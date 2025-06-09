using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Personnel
{
    public class PersonnelAvailabilityModel
    {
        public long PersonnelId { get; set; }
        [Required(ErrorMessage = "AvailabilityDateErrMsg|" + LocalizeResourceSetConstants.PersonnelDetails)]
        public DateTime? PersonnelAvailabilityDate { get; set; }

        [Required(ErrorMessage = "AvailabilityHourErrMsg|" + LocalizeResourceSetConstants.PersonnelDetails),
         RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global),
         Range(0.1, 999999.99, ErrorMessage = "GT0.0MinTwoDecimalAfterSixDecimalBeforeTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal PAHours { get; set; }
        public string PAShift { get; set; }
        public long? PersonnelAvailabilityId { get; set; }
        public string ClientLookupId { get; set; }
    }
}