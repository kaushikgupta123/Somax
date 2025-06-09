using Client.Models.Common;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.GuestWorkRequest
{
    public class GuestWorkRequestModel
    {
        [Required(ErrorMessage = "validationRequestorName|" + LocalizeResourceSetConstants.WorkOrderDetails)]
        public string RequestorName { get; set; }
        public string RequestorPhoneNumber { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "GlobalEmailErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string RequestorEmail { get; set; }
        public string Type { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
    }
}