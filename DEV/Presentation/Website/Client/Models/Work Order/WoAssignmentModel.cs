using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Work_Order
{
    public class WoAssignmentModel
    {
        public long ClientId { get; set; }
        public string ClientLookupId { get; set; }
        public long WorkOrderSchedId { get; set; }
        public long WorkOrderId { get; set; }
        public string AssignedTo_PersonnelClientLookupId { get; set; }
        public string WorkAssigned_Name { get; set; }
        [Display(Name = "spnPersonnelID|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public long PersonnelId { get; set; }
        public bool Rescheduled { get; set; }
      
        [Display(Name = "spnDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "vaildDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? ScheduledStartDate { get; set; }
        [Display(Name = "spnHours|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 999999.99, ErrorMessage = "globalTwoDecimalAfterTotalEightRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? ScheduledHours { get; set; }
        public int UpdateIndex { get; set; }
        public IEnumerable<SelectListItem> PersonnelIdList { get; set; }
        public IEnumerable<SelectListItem> WorkAssignedLookUpList { get; set; }
    }
}