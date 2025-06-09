using Client.CustomValidation;

using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.WorkOrderPlanning
{
    public class WorkOrderPlanningModel
    {
        public long WorkOrderPlanId { get; set; }
        public long PlanID { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "StartDateErrorMessage|" + LocalizeResourceSetConstants.WorkOrderPlanning)]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "EndDateErrorMessage|" + LocalizeResourceSetConstants.WorkOrderPlanning)]
        [DateGreaterThanAttribute(otherPropertyName = "StartDate", ErrorMessage = "EndDateMustBeGreaterThanStartDateErrorMessage|" + LocalizeResourceSetConstants.WorkOrderPlanning)]
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Completed { get; set; }
        public Int32 ChildCount { get; set; }
        public Int32 TotalCount { get; set; }
        public IEnumerable<SelectListItem> PersonnelList { get; set; }
        public long? PersonnelId { get; set; }

    }
}