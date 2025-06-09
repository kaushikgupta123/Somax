using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class WoEmergencyDescribeModel
    {
        public long? DepartmentId { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }

        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
  

        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }

        [Display(Name = "Charge To")]
        public long? ChargeTo { get; set; }
        public IEnumerable<SelectListItem> ChargeToList { get; set; }

        [Required(ErrorMessage = "validationChargeTo|" + LocalizeResourceSetConstants.Global)] //V2-952
        public string ChargeToClientLookupId { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]


        public string Description { get; set; }
        public bool IsDepartmentShow { get; set; }
        public bool IsTypeShow { get; set; }
        public bool IsDescriptionShow { get; set; }
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
    }

    public class WoEmergencyOnDamandModel
    {
        public long? DepartmentId { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }

        [Required(ErrorMessage = "spnReqOnDemandProcedure|" + LocalizeResourceSetConstants.Global)]
        public string OnDemandID { get; set; }
        public IEnumerable<SelectListItem> OnDemandIDList { get; set; }

        [Required(ErrorMessage = "spnWORequestType|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        // Commented for V2-608
        //[Required(ErrorMessage = "PrLiChargeType|" + LocalizeResourceSetConstants.SanitationDetails)]
        //[Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        //public string ChargeType { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeToList { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "validationChargeTo|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToClientLookupId { get; set; }
        public bool IsDepartmentShow { get; set; }
        public bool IsTypeShow { get; set; }
        public bool IsDescriptionShow { get; set; }
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        
    }
}