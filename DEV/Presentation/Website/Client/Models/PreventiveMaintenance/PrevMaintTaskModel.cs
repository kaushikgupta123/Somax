using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.PreventiveMaintenance
{
    public class PrevMaintTaskModel
    {
        public long PrevMaintTaskId { get; set; }
        public long PrevMaintMasterId { get; set; }
        public long AssignedTo_PersonnelId { get; set; }
        public long ChargeToId { get; set; }
        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }
        public string ChargeTypeText { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public long MeterId { get; set; }
        public string ReadingType { get; set; }
        public decimal ScheduledDuration { get; set; }
        [Display(Name = "spnOrder|" + LocalizeResourceSetConstants.Global)]
        public string TaskNumber { get; set; }
        public string Type { get; set; }
        public string AssignedTo_ClientLookupId { get; set; }
        [Display(Name = "GlobalChargeTo|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToClientLookupId { get; set; }
        public long updatedindex { get; set; }
        public bool Security { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }

        public string PrevmaintClientlookUp { get; set; }
    }
}