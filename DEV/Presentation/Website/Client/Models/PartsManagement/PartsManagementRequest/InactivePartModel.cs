using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class InactivePartModel
    {
        [Required(ErrorMessage = "ValidPartToInactive|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string PartToInactivate { get; set; }
        [Required(ErrorMessage = "ValidJustification|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string Justification { get; set; }
        public string RequestType { get; set; }
    }
}