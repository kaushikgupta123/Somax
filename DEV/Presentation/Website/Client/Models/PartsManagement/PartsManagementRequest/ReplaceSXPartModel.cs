using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class ReplaceSXPartModel
    {
        [Required(ErrorMessage = "ValidSXParttoreplace|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string SXPartToReplace { get; set; }
        [Required(ErrorMessage = "ValidParttoreplacewith|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string ReplaceWith { get; set; }
        [Required(ErrorMessage = "ValidJustification|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string Justification { get; set; }
        public string RequestType { get; set; }
    }
}