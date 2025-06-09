using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class WoTaskModel
    {
        public long WorkOrderTaskId { get; set; }
        public string ClientLookupId { get; set; }
        public long WorkOrderId { get; set; }
        public long AssignedTo_PersonnelId { get; set; }
        public long ChargeToId { get; set; }
        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }
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
        public int updatedindex { get; set; }
        public bool Security { get; set; }
        [Display(Name = "spnStatus|" + LocalizeResourceSetConstants.Global)]
        public string Status { get; set; }
        [Display(Name = "spnCompletedBy|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public long? CompleteBy_PersonnelId { get; set; }
        public string CompleteBy_PersonnelClientLookupId { get; set; }
        [Display(Name = "spnGlobalCompleted|" + LocalizeResourceSetConstants.Global)]
        public DateTime? CompleteDate { get; set; }
        [Display(Name = "spnCancelReason|" + LocalizeResourceSetConstants.Global)]
        public string CancelReason { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }

        public IEnumerable<SelectListItem> CancelReasonList { get; set; }
    }
}