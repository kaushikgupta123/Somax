using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class SanitationDescribeWoModel
    {
        [Required(ErrorMessage = "spnReqOnDemandProcedure|" + LocalizeResourceSetConstants.Global)]
        public long? OnDemandId { get; set; }
        
        [Required(ErrorMessage = "validationRequiredDate|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        [Display(Name = "spnRequired|" + LocalizeResourceSetConstants.Global)]
        public DateTime? Required { get; set; }

        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public IEnumerable<SelectListItem> OnDemandList { get; set; }
       
    }
}