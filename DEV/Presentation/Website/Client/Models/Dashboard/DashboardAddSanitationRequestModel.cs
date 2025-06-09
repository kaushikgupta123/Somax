using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Dashboard
{
    public class DashboardAddSanitationRequestModel
    {

        [Display(Name = "spnChargeType|" + LocalizeResourceSetConstants.Global)]
        public string ChargeType { get; set; }

        [Display(Name = "Charge To")]
        public long? ChargeTo { get; set; }
       
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }

        [Required(ErrorMessage = "validationChargeTo|" + LocalizeResourceSetConstants.Global)]
        public string ChargeToClientLookupId { get; set; }
    }
}