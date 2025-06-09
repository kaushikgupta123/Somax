using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class CompletionLaborWizard
    {

        [Display(Name = "spnPersonnelID|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        [Required(ErrorMessage = "validpersonnelid|" + LocalizeResourceSetConstants.WorkOrderDetails)]        
        public long? PersonnelID { get; set; }
        [Display(Name = "spnDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnReqDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "spnHours|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "vaildHours|" + LocalizeResourceSetConstants.WorkOrderDetails)]      
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        //[Range(double.Epsilon, 99999999.99, ErrorMessage = "globalGreaterThan0TwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(double.Epsilon, 23.99, ErrorMessage = "globalValidHoursGreaterThan0AndlessThan0OrEqualTo23.99RegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Hours { get; set; }
        //public long workOrderId { get; set; }
        //public long TimecardId { get; set; }
        //public string PersonnelClientLookupId { get; set; }
        //public string NameFull { get; set; }    
        //public decimal TCValue { get; set; }
        public IEnumerable<SelectListItem> WorkAssignedLookUpList { get; set; }
    }
}