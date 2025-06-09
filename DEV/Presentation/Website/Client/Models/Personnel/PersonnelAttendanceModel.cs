using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Personnel
{
    public class PersonnelAttendanceModel
    {
        public long PersonnelId { get; set; }
        public long PersonnelAttendanceId { get; set; }
        public string PersonnelClientLookupId { get; set; }
        [Required(ErrorMessage = "AttendanceDateErrMsg|" + LocalizeResourceSetConstants.PersonnelDetails)]
        public DateTime? Date { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.1, 999999.99, ErrorMessage = "GT0.0MinTwoDecimalAfterSixDecimalBeforeTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "AttendanceHourErrMsg|" + LocalizeResourceSetConstants.PersonnelDetails)]
        public Decimal Hours { get; set; }
        public string Shift { get; set; }
        public string ShiftDescription { get; set; }
    }
}