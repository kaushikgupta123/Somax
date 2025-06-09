using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class WoOnDemandModel
    {
        public string ClientLookupId { get; set; }
        //[Display(Name = "validationOnDemandProcedure|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        [Required(ErrorMessage = "validationOnDemandProcedure|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string MaintOnDemandClientLookUpId { get; set; }
        //[Required(ErrorMessage = "Please select Type")]
        [Required(ErrorMessage = "spnWORequestType|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string Type { get; set; }
        //[Required(ErrorMessage = "Please select Charge Type")]
        // Commented for V2-608
        //[Required(ErrorMessage = "validationChargeType|" + LocalizeResourceSetConstants.Global)]
        //public string ChargeType { get; set; }
        //[Required(ErrorMessage = "Please select Charge To")]        
        [Required(ErrorMessage = "validationChargeTo|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToClientLookupId { get; set; }
        public string Description { get; set; }
        public IEnumerable<SelectListItem> MaintOnDemandList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeTypelookUpList { get; set; }
        public IEnumerable<SelectListItem> PriorityList { get; set; }
        [Display(Name = "spnPriority|" + LocalizeResourceSetConstants.Global)]
        public string Priority { get; set; }
        [Display(Name = "spnRequired|" + LocalizeResourceSetConstants.Global)]
        public DateTime? RequiredDate { get; set; }
    }
}