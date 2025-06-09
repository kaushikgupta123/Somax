using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Sanitation
{
    public class SanitationJobTaskModel
    {
        public string ClientLookupId { get; set; }
        public long ClientId { get; set; }
        public long SanitationJobTaskId { get; set; }
        public long SanitationJobId { get; set; }
        public long SanitationMasterTaskId { get; set; }       
        public string CancelReason { get; set; }
        public long? CompleteBy_PersonnelId { get; set; }              
        [Display(Name = "spnCompletedBy" )]
        public string CompleteBy_PersonnelClientLookupId { get; set; }
        public string CompleteBy { get; set; }
        public string CompleteComments { get; set; }
        [Display(Name = "spnCompleteDate")]
        public DateTime? CompleteDate { get; set; }       
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnReqDescription|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string Description { get; set; }
        public string Status { get; set; }
        [Display(Name = "Order")]
        [Required(ErrorMessage = "spnTaskIdRequired|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string TaskId { get; set; }
        public string RecordedValue { get; set; }
        public long SanOnDemandMasterTaskId { get; set; }
        public int UpdateIndex { get; set; }
        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }
        [Display(Name = "GlobalChargeTo|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToClientLookupId { get; set; }

        public IEnumerable<SelectListItem> CancelReasonList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }

        public int CompleteStatus { get; set; }
    }
}