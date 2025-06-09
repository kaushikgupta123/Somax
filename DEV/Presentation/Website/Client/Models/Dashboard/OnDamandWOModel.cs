using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Dashboard
{
    public class OnDamandWOModel
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
        public IEnumerable<SelectListItem> ChargeTypeList { get; set; }
        public IEnumerable<SelectListItem> ChargeToList { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "validationChargeTo|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToClientLookupId { get; set; }
        public long? ChargeTo { get; set; }
        public bool IsDepartmentShow { get; set; }
        public bool IsTypeShow { get; set; }
        public bool IsDescriptionShow { get; set; }
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
    }
}