using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.FleetService
{
    public class ServiceOrderScheduleModel
    {
        public long PersonnelId { get; set; }
        public IEnumerable<SelectListItem> Personnellist { get; set; }
        [FutureDateValidation(ErrorMessage = "ScheduledPastDateErrorMessage|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        [Required(ErrorMessage = "SelectAssignedErrorMessage|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public DateTime? Schedulestartdate { get; set; }
        public decimal ScheduledDuration { get; set; }
        public long ServiceOrderId { get; set; }
        [Required(ErrorMessage = "SelectAssignedErrorMessage|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public List<string> PersonnelIds { get; set; }
        public string ServiceOrderIds { get; set; }
        public long AssignedPersonnelId { get; set; }
        public IEnumerable<SelectListItem> Assignedlist { get; set; }
        public string ClientLookupIds { get; set; }
        public string Status { get; set; }
    }
}