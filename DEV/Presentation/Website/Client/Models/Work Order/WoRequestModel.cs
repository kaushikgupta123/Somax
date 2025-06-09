using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class WoRequestModel
    {
      
      
        public long? DepartmentId { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }



      //[Required(ErrorMessage = "Please Select Type")]
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }


     // [Required(ErrorMessage = "Please Select Charge Type")]
        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }


     // [Required(ErrorMessage = "Please Select Charge To")]
        [Display(Name = "Charge To")]
        public long? ChargeTo { get; set; }
        public IEnumerable<SelectListItem> ChargeToList { get; set; }
        public string ChargeToClientLookupId { get; set; }

        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }

        public bool IsDepartmentShow { get; set; }
        public bool IsTypeShow { get; set; }
        public bool IsDescriptionShow { get; set; }
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public bool PlantLocation { get; set; }
    }
}