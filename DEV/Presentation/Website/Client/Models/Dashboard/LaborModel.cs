using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Dashboard
{
    public class LaborModel
    {
        [Required(ErrorMessage = "validpersonnelid|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public long? PersonnelID { get; set; }
        [Required(ErrorMessage = "spnReqDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "vaildHours|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        //[Range(double.Epsilon, 99999999.99, ErrorMessage = "globalGreaterThan0TwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        //[RegularExpression(@"^[0-9]*$", ErrorMessage = "globalValidHoursGreaterThan0AndlessThan0OrEqualTo24RegErrMsg|" + LocalizeResourceSetConstants.Global)]
        //[Range(1, 24, ErrorMessage = "globalValidHoursGreaterThan0AndlessThan0OrEqualTo24RegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 23.99, ErrorMessage = "globalValidHoursGreaterThan0AndlessThan0OrEqualTo23.99RegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Hours { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public long TimecardId { get; set; }
        public int TotalCount { get; set; }
        public long ServiceOrderLineItemId { get; set; }
        public long WorkOrderId { get; set; }
    }
}